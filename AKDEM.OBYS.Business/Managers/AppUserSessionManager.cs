using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
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
    public class AppUserSessionManager : GenericManager<AppUserSessionCreateDto, AppUserSessionUpdateDto, AppUserSessionListDto, AppUserSession>, IAppUserSessionService
    {
        private readonly IMapper _mapper;
        private readonly IUow _uow;
        private readonly IAppBranchService _appBranchService;
        private readonly IAppUserSessionLessonService _appUserSessionLessonService;
        public AppUserSessionManager(IMapper mapper, IValidator<AppUserSessionCreateDto> createDtoValidator, IValidator<AppUserSessionUpdateDto> updateDtovalidator, IUow uow, IAppBranchService appBranchService, IAppUserSessionLessonService appUserSessionLessonService) : base(mapper, createDtoValidator, updateDtovalidator, uow)
        {
            _mapper = mapper;
            _uow = uow;
            _appBranchService = appBranchService;
            _appUserSessionLessonService = appUserSessionLessonService;
        }
        public async Task CreateUserSessionAsync(List<AppUserSessionCreateDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var entity = _mapper.Map<AppUserSession>(dto);
                await _uow.GetRepositry<AppUserSession>().CreateAsync(entity);
                await _uow.SaveChangesAsync();
            }
        }
        public async Task<bool> CreateUserSessionByUserIdAsync(List<AppUserSessionCreateDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var entity = _mapper.Map<AppUserSession>(dto);
                var result = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.SessionId == dto.SessionId && x.UserId == dto.UserId);
                if (result == null)
                {
                    await _uow.GetRepositry<AppUserSession>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                    return true;
                }
               
            }
            return false;

        }
        public async Task<List<int>> GetUserSessionsByBranchId(int sessionId, int branchId)
        {
            List<int> userSessionIds = new();
            var userSessions = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.AppUser.BranchId == branchId);
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var usersessions = await query.Where(x => x.SessionId == sessionId && x.AppUser.BranchId == branchId).ToListAsync();
            foreach (var userSession in usersessions)
            {
                userSessionIds.Add(userSession.Id);
            }
            var mappedUserSessions = _mapper.Map<List<AppUserSessionListDto>>(usersessions);
            return userSessionIds;
        }
        public async Task<List<int>> GetUserSessionIdsByUserIdAsync(int userId)
        {
            List<int> userSessionIds = new();
            //var userSessions = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.UserId == userId && x.AppSession.AppSessionBranches.Any());
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var userSessions = await query.Include(x => x.AppSession).Where(x => x.UserId == userId ).ToListAsync();
            if (userSessions.Count != 0)
            {
                foreach (var item in userSessions)
                {
                    userSessionIds.Add(item.Id);
                }
            }
            
            return userSessionIds;
        }
        public async Task RemoveUserSessions(List<int> userSessionList)
        {
            if (userSessionList.Count != 0)
            {
                foreach (var item in userSessionList)
                {
                    var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == item);
                    if (entity != null)
                    {
                        _uow.GetRepositry<AppUserSession>().Remove(entity);
                        await _uow.SaveChangesAsync();
                    }

                }
            }
        }
        public async Task SetZeroToSessionAverage(int userId, int sessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.UserId == userId && x.SessionId == sessionId);
            if (entity != null)
            {

                entity.Average = 0;

                await _uow.SaveChangesAsync();
            }
        }
        public async Task SetZeroToSessionAverageBySessionId(int sessionId)
        {
            var entities = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.SessionId == sessionId);
            if (entities.Count != 0)
            {
                foreach (var entity in entities)
                {
                    entity.Average = 0;
                    await _uow.SaveChangesAsync();
                }
            }

        }
        public async Task FindAverageOfSessionWithUserAndSession(int userId, int sessionId)
        {
            double not = 0;
            int count = 0;
            int count2 = 0;
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.UserId == userId && x.SessionId == sessionId);


            if (entity != null)
            {
                var entities = await _uow.GetRepositry<AppUserSessionLesson>().GetAllAsync(x => x.UserSessionId == entity.Id);
                if (entities.Count != 0)
                {
                    foreach (var item in entities)
                    {
                        if (item.Not != -1 && item.Not != -5 && item.Not!= -6)
                        {
                            not = not + item.Not;
                            count = count + 1;

                        }
                        else
                        {
                            count2 = count2 + 1;
                            
                        }
                    }
                    if (count != 0)
                    {
                        entity.Average = not / count;

                        await _uow.SaveChangesAsync();

                    }
                    if(count2 == entities.Count)
                    {
                        entity.Average = -1;

                        await _uow.SaveChangesAsync();
                    }
                    
                }
                

            }



        }
        public async Task TotalAverageByUserId(int userId, int sessionId)
        {
            var user = await _uow.GetRepositry<AppUser>().FindByFilterAsync(x => x.Id == userId);

            double totalAverage = 0;
            double sum = 0;
            List<double> averages = new();
            var entities = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.UserId == userId && x.Average!=-1 && x.SessionId <=sessionId);
            if (entities.Count != 0)
            {
                foreach (var entity in entities)
                {
                    averages.Add(entity.Average);
                }
                foreach (var item in averages) { sum = sum + item; }

                totalAverage = sum / entities.Count;
                user.TotalAverage = totalAverage;
            }
            else
            {
                user.TotalAverage = 0;
            }
            await _uow.SaveChangesAsync();


        }
        public async Task TotalAverageAllUsers(int sessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var users = await query.Where(x => x.AppUserRoles.Any(x => x.RoleId == 3)).ToListAsync();
            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    await TotalAverageByUserId(user.Id,sessionId );
                }
            }

        }
        public async Task TotalAverageAllUsersGeneral()
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var users = await query.Where(x => x.AppUserRoles.Any(x => x.RoleId == 3)).ToListAsync();
            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    await TotalAverageByUserIdGeneral(user.Id);
                }
            }

        }
        public async Task TotalAverageByUserIdGeneral(int userId)
        {
            var user = await _uow.GetRepositry<AppUser>().FindByFilterAsync(x => x.Id == userId);

            double totalAverage = 0;
            double sum = 0;
            List<double> averages = new();
            var entities = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.UserId == userId && x.Average != -1);
            if (entities.Count != 0)
            {
                foreach (var entity in entities)
                {
                    averages.Add(entity.Average);
                }
                foreach (var item in averages) { sum = sum + item; }

                totalAverage = sum / entities.Count;
                user.TotalAverage = totalAverage;
            }
            else
            {
                user.TotalAverage = 0;
            }
            await _uow.SaveChangesAsync();


        }
        public async Task<int> UserSessionIdByUserIdAndSessionId(int userId, int sessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.UserId == userId && x.SessionId == sessionId);
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<bool> IfThereIsAnyUserSession(int userId, int sessionId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.UserId == userId && x.AppSession.Id == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return true;
            }
            return false;
        }
        public async Task<int> GetSessionIdByUserSessionId(int userSessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == userSessionId);
            if (entity != null)
            {
                return entity.SessionId;
            }
            return 0;
        }
        public async Task<int> GetBranchIdByUserSessionId(int userSessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == userSessionId);
            if (entity != null)
            {
                return entity.BranchId;
            }


            return 0;

        }
        public async Task<int> GetClassIdByUserSessionId(int userSessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == userSessionId);
            if (entity != null)
            {
                return entity.ClassId;
            }


            return 0;

        }
        public async Task<bool> GetUserSessionStatus(int userSessionId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.Id == userSessionId).SingleOrDefaultAsync();
            
            if (entity != null)
            {
                return entity.Status;
            }


            return true;

        }
        public async Task RemoveUserSessionBySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entities = await query.Where(x => x.AppSession.Id == sessionId).ToListAsync();
            //var entities = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.AppSession.Id == sessionId);
            if (entities.Count != 0)
            {
                foreach (var item in entities)
                {
                    _uow.GetRepositry<AppUserSession>().Remove(item);
                }
                await _uow.SaveChangesAsync();
            }
        }
        public async Task RemoveUserSessionByUserId(int userId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entities = await query.Where(x => x.UserId == userId).ToListAsync();
            
            if (entities.Count != 0)
            {
                foreach (var item in entities)
                {
                    _uow.GetRepositry<AppUserSession>().Remove(item);
                }
                await _uow.SaveChangesAsync();
            }
        }
        public async Task<int> GetUserIdByUserSessionId(int userSessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == userSessionId);
            if (entity != null)
            {
                return entity.UserId;

            }
            return 0;
        }
        public async Task<string> GetUserNameByUserSessionId(int userSessionId)
        {
            var entity = await _uow.GetRepositry<AppUserSession>().FindByFilterAsync(x => x.Id == userSessionId);
            var user = await _uow.GetRepositry<AppUser>().FindByFilterAsync(x => x.Id == entity.UserId);
            return user.FirstName + " " + user.SecondName;
        }
        public async Task<double> ReturnTotalAverage(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.TotalAverage;

            }
            return 0;
        }
        public async Task RemoveUserSessionsAndLessons(int sessionId, int userId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.AppSession.Id == sessionId && x.AppUser.Id == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                await _appUserSessionLessonService.RemoveUserSessionLessonsByUserSessionId(entity.Id);
                _uow.GetRepositry<AppUserSession>().Remove(entity);
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<double> ReturnSessionAverage(int userSessionId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.Id == userSessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Average;

            }
            return 0;
        }
        public async Task<int> ReturnSessionOrderOfBranch(int userId, int branchId, int sessionId)
        {
            int degree = 1;
            var userSessions = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.Average, x => x.AppUser.BranchId == branchId && x.SessionId == sessionId, Common.Enums.OrderByType.DESC);
            if (userSessions.Count != 0)
            {
                for( int i=0;i < userSessions.Count; i++)
                {
                    if (i == 0)
                    {
                        if (userSessions[i].Average <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }
                    
                    else
                    {
                        if(userSessions[i-1].Average!= userSessions[i].Average)
                        {
                            degree = degree + 1;
                        }
                        if (userSessions[i].Average <= 0)
                        {
                            degree = 0;
                        }
                        
                    }
                    if (userSessions[i].UserId == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;

        }
        public async Task<int> ReturnTotalOrderOfBranch(int userId, int branchId,int sessionId)
        {
            var users = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.TotalAverage, x => x.BranchId == branchId &&x.Status==true && x.AppUserSessions.Any(x=>x.SessionId<=sessionId), Common.Enums.OrderByType.DESC);
            int degree = 1;

            if (users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                    {
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }
                   
                    else
                    {
                        if (users[i - 1].TotalAverage != users[i].TotalAverage)
                        {
                            degree = degree + 1;
                        }
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }

                    }
                    if (users[i].Id == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;


        }
        public async Task<int> ReturnTotalOrderOfBranchGeneral(int userId, int branchId)
        {
            var users = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.TotalAverage, x => x.BranchId == branchId && x.Status == true , Common.Enums.OrderByType.DESC);
            int degree = 1;

            if (users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                    {
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }

                    else
                    {
                        if (users[i - 1].TotalAverage != users[i].TotalAverage)
                        {
                            degree = degree + 1;
                        }
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }

                    }
                    if (users[i].Id == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;


        }
        public async Task<int> ReturnSessionOrderOfClass(int userId, int branchId, int sessionId)
        {
            int classId = await _appBranchService.GetClassIdByBranchId(branchId);
            var userSessions = await _uow.GetRepositry<AppUserSession>().GetAllAsync(x => x.Average, x => x.AppUser.ClassId==classId && x.SessionId==sessionId, Common.Enums.OrderByType.DESC);
            int degree = 1;
            if (userSessions.Count != 0)
            {
                for (int i = 0; i < userSessions.Count; i++)
                {
                    if (i == 0 )
                    {
                        if (userSessions[i].Average <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }
                    
                    else
                    {
                        if (userSessions[i - 1].Average != userSessions[i].Average)
                        {
                            degree = degree + 1;
                        }
                        if (userSessions[i].Average <= 0)
                        {
                            degree = 0;
                        }

                    }
                    if (userSessions[i].UserId == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;


        }
        public async Task<int> ReturnTotalOrderOfClass(int userId, int branchId,int sessionId)
        {
            int classId = await _appBranchService.GetClassIdByBranchId(branchId);

            var users = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.TotalAverage, x => x.ClassId == classId && x.Status==true && x.AppUserSessions.Any(x=>x.SessionId<=sessionId), Common.Enums.OrderByType.DESC);

            int degree = 1;

            if (users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                    {
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }
                   
                    else
                    {
                        if (users[i - 1].TotalAverage != users[i].TotalAverage)
                        {
                            degree = degree + 1;
                        }
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }

                    }
                    if (users[i].Id == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;


        }
        public async Task<int> ReturnTotalOrderOfClassGeneral(int userId, int branchId)
        {
            int classId = await _appBranchService.GetClassIdByBranchId(branchId);

            var users = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.TotalAverage, x => x.ClassId == classId && x.Status == true, Common.Enums.OrderByType.DESC);

            int degree = 1;

            if (users.Count != 0)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                    {
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }
                        else
                        {
                            degree = 1;

                        }
                    }

                    else
                    {
                        if (users[i - 1].TotalAverage != users[i].TotalAverage)
                        {
                            degree = degree + 1;
                        }
                        if (users[i].TotalAverage <= 0)
                        {
                            degree = 0;
                        }

                    }
                    if (users[i].Id == userId)
                    {
                        break;
                    }
                }
                return degree;
            }
            return 0;


        }
        public async Task<string> ReturnPassiveDateByUserId(int userId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Include(x => x.AppSession).Where(x => x.Status == false && x.AppUser.Id == userId).OrderBy(x => x.Id).LastOrDefaultAsync();
            if (entity != null)
            {
                return entity.AppSession.Definition;
            }
            return "";
        }
        public async Task<string> LastBranchNameByUserAndSessionId(int sessionId, int userId)
        {
            var query = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Include(x => x.AppBranch).Where(x => x.UserId == userId && x.SessionId == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.AppBranch.Definition;
            }
            return "";
        }
    }
}
