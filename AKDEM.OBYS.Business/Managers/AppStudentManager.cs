using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Managers
{
    public class AppStudentManager : GenericManager<AppStudentCreateDto, AppStudentUpdateDto, AppStudentListDto, AppUser>, IAppStudentService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AppStudentCreateDto> _createDtoValidator;
        private readonly IValidator<AppStudentUpdateDto> _updateDtoValidator;
        private readonly IUow _uow;
        private readonly IAppSessionService _appSessionService;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        private readonly IAppScheduleService _appScheduleService;
        private readonly IAppScheduleDetailService _appScheduleDetailService;
        private readonly IAppBranchService _appBranchService;

        public AppStudentManager(IMapper mapper, IValidator<AppStudentCreateDto> createDtoValidator, IValidator<AppStudentUpdateDto> updateDtoValidator, IUow uow, IValidator<AppStudentUpdateDto> createStudentDtoValidator, IAppSessionService appSessionService, IAppUserSessionService appUserSessionService, IAppUserSessionLessonService appUserSessionLessonService, IAppScheduleService appScheduleService, IAppScheduleDetailService appScheduleDetailService, IAppBranchService appBranchService) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _uow = uow;
            _updateDtoValidator = createStudentDtoValidator;
            _appSessionService = appSessionService;
            _appUserSessionService = appUserSessionService;
            _appUserSessionLessonService = appUserSessionLessonService;
            _appScheduleService = appScheduleService;
            _appScheduleDetailService = appScheduleDetailService;
            _appBranchService = appBranchService;
        }
        public async Task<List<AppStudentListDto>> GraduatedStudenstBySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var students = await query.Where(x => x.AppUserSessions.Any(x => x.ClassId == (int)ClassType.Dört && x.SessionId == sessionId && x.Status == true) && x.Status == true).ToListAsync();
            if (students.Count != 0)
            {
                var descList = students.OrderByDescending(x => x.TotalAverage).ToList();
                var mappedList = _mapper.Map<List<AppStudentListDto>>(descList);
                return mappedList;
            }
            return new List<AppStudentListDto>();

        }
        public async Task<string> ReturnDepartReasonOfStudent(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                return user.DepartReason;
            }
            return "";
        }
        public async Task<bool> ReturnStatusOfStudent(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                return user.Status;
            }
            return false;
        }


        public async Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserRoles.Any(x => x.RoleId == (int)type) && x.AppBranch.Status == true).ToListAsync();
            return _mapper.Map<List<AppStudentListDto>>(list);
        }
        public async Task<IResponse<AppStudentCreateDto>> CreateStudentWithRoleAsync(AppStudentCreateDto dto, int roleId)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = _mapper.Map<AppUser>(dto);
                await _uow.GetRepositry<AppUser>().CreateAsync(user);
                await _uow.GetRepositry<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppUser = user,
                    RoleId = roleId
                });
                await _uow.SaveChangesAsync();
                return new Response<AppStudentCreateDto>(ResponseType.Success, dto);

            }
            return new Response<AppStudentCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<List<AppStudentListDto>> GetStudentsWithBranchAsync(int id)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.BranchId == id).ToListAsync();
            if (list.Count != 0)
            {
                var descList = list.OrderByDescending(x => x.TotalAverage).ToList();
                return _mapper.Map<List<AppStudentListDto>>(descList);
            }
            return new List<AppStudentListDto>();
        }
        public async Task<List<AppStudentListDto>> GetStudentsWithBranchAndSessionAsync(int branchId, int sessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query
    .Where(x => x.AppUserSessions.Any(s => s.BranchId == branchId && s.SessionId == sessionId))
    .Include(x => x.AppBranch)
        .ThenInclude(x => x.AppClass)
    .ToListAsync();
            var descList = list.OrderByDescending(x => x.TotalAverage).ToList();
            return _mapper.Map<List<AppStudentListDto>>(descList);
        }
        public async Task<List<AppStudentListDto>> GetStudentsWithBranchAndSessionAndLessonAsync(int branchId, int sessionId, int lessonId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserSessions.Any(x => x.BranchId == branchId) && x.AppUserSessions.Any(x => x.SessionId == sessionId)).ToListAsync();
            var descList = list
                .OrderByDescending(x => x.AppUserSessions != null &&
                                        x.AppUserSessions.Any(s => s != null && s.SessionId == sessionId &&
                                                                    s.AppUserSessionLessons != null &&
                                                                    s.AppUserSessionLessons.Any(lesson => lesson != null && lesson.Id == lessonId)))
                .ThenByDescending(x => x.AppUserSessions?.Where(s => s != null && s.SessionId == sessionId && s.AppUserSessionLessons != null)
                                        .SelectMany(s => s.AppUserSessionLessons)
                                        .Where(lesson => lesson != null && lesson.Id == lessonId)
                                        .Select(lesson => lesson.Not)
                                        .FirstOrDefault())
                .ToList();





            return _mapper.Map<List<AppStudentListDto>>(descList);
        }








        public async Task<AppStudentListDto> GetStudentById(int id)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var entity = await query.Include(x => x.AppBranch).Include(x => x.AppClass).Where(x => x.Id == id).SingleOrDefaultAsync();
            var student = _mapper.Map<AppStudentListDto>(entity);
            return student;
        }
        public async Task ChangeStudentStatusBecasuseOfWarning(int userId, double sessionWarningCount, double totalWarningCount)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (sessionWarningCount >= 2 || totalWarningCount >= 3)
            {
                entity.Status = false;

            }
            else
            {
                entity.Status = true;
            }
            await _uow.SaveChangesAsync();

        }
        public async Task<List<AppStudentListDto>> GetPassiveStudents(int sessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var users = await query.Include(x => x.AppBranch).Where(x => x.AppUserSessions.Any(x => x.SessionId == sessionId) && x.Status == false).ToListAsync();

            if (users.Count != 0)
            {
                var mappedUsers = _mapper.Map<List<AppStudentListDto>>(users);
                return mappedUsers;
            }
            return new List<AppStudentListDto>();
        }
        public async Task CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                int branchId = (int)user.BranchId;
                int classId = await _appBranchService.GetClassIdByBranchId(branchId);
                List<AppUserSessionCreateDto> userSessionCreateDtos = new();
                var sessions = await _appSessionService.GetAllAsync();
                var branch = await _appBranchService.GetBrancWithId(branchId);

                foreach (var session in sessions.Data)
                {
                    if (session.Status2 == true && branch.Status == true)
                    {
                        userSessionCreateDtos.Add(new AppUserSessionCreateDto { BranchId = branchId, ClassId = classId, SessionId = session.Id, UserId = userId });
                    }
                }
                bool result = await _appUserSessionService.CreateUserSessionByUserIdAsync(userSessionCreateDtos);
                if (result)
                {
                    var control = await _appUserSessionLessonService.IsThereAnyScheduleForThisUser(branchId);
                    if (control)
                    {
                        List<AppUserSessionLessonCreateDto> userSessionLessonCreateDtos = new();
                        var scheduleId = await _appUserSessionLessonService.GetScheduleIdForThisUser(branchId);
                        var lessons = await _appScheduleDetailService.GetLessonsByScheduleIdAsync(scheduleId);
                        var userSessions = await _appUserSessionService.GetUserSessionIdsByUserIdAsync(userId);
                        foreach (var lesson in lessons)
                        {
                            foreach (var userSession in userSessions)
                            {
                                userSessionLessonCreateDtos.Add(new AppUserSessionLessonCreateDto { LessonId = lesson, UserSessionId = userSession });
                            }
                        }
                        await _appUserSessionLessonService.CreateUserSessionLessonAsyncByUserSessions(userSessionLessonCreateDtos);
                    }
                }

            }



        }

        public async Task<List<AppStudentListDto>> GetStudentsByClassId(RoleType type, int classId)
        {

            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserRoles.Any(x => x.RoleId == (int)type) && x.ClassId == classId).ToListAsync();
            return _mapper.Map<List<AppStudentListDto>>(list);
        }
        public async Task ControlUserSessionWhenUpdating(int userId, int sessionId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.SessionId == sessionId && x.UserId == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                var query2 = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
                var entities = await query2.Where(x => x.UserSessionId == entity.Id).ToListAsync();
                if (entities.Count != 0)
                {
                    foreach (var item in entities)
                    {
                        _uow.GetRepositry<AppUserSessionLesson>().Remove(item);

                    }
                }
                _uow.GetRepositry<AppUserSession>().Remove(entity);
                await _uow.SaveChangesAsync();
            }
        }
        public async Task ChangeStudentBranch(int studentId, int branchId, int classId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == studentId).SingleOrDefaultAsync();
            if (user != null)
            {
                user.BranchId = branchId;
                user.ClassId = classId;
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<string> ReturnUserImg(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                if (user.ImagePath != null)
                {
                    var ImageStr = user.ImagePath.Replace("\\", "/");
                    return ImageStr;

                }



            }
            return "/images/user.png";
        }
        public async Task ChangeBranchForStudent(int userId, int newBranchId)
        {
            var query =  _uow.GetRepositry<AppUser>().GetQuery();
            var users = await query.Where(x => x.Id == userId).ToListAsync();
            if (users.Count != 0)
            {
                foreach(var user in users)
                {
                    user.BranchId = newBranchId;
                }
            }
            await _uow.SaveChangesAsync();

        }
    }
}
