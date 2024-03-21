using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Dto.AppScheduleDtos;
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
    public class AppScheduleManager : GenericManager<AppScheduleCreateDto, AppScheduleUpdateDto, AppScheduleListDto, AppSchedule>, IAppScheduleService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<AppScheduleCreateDto> _createDtoValidator;

        public AppScheduleManager(IMapper mapper, IValidator<AppScheduleCreateDto> createDtoValidator, IValidator<AppScheduleUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
        }

        public async Task<List<AppScheduleListDtoDeveloper>> GetAppSchedulesDeveloper()
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entites = await query.ToListAsync();
            if (entites.Count != 0)
            {
                var lists = _mapper.Map<List<AppScheduleListDtoDeveloper>>(entites);
                return lists;
            }
            return new List<AppScheduleListDtoDeveloper>();
        }
        public async Task<List<AppScheduleListDto>> GetSchedulesBySession(int id)
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            
            var schedules = await query.Include(x => x.AppSessionBranch).ThenInclude(x=>x.AppSession).Include(x=>x.AppSessionBranch).ThenInclude(x=>x.AppBranch).Where(x => x.AppSessionBranch.SessionId == id).ToListAsync();
            if (schedules.Count != 0)
            {
                return _mapper.Map<List<AppScheduleListDto>>(schedules);

            }
            return new List<AppScheduleListDto>();
            //var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            //var data = await query.Include(x => x.AppSessionBranch).ThenInclude(x=>x.AppBranch).


        }
        public async Task<string> GetNameByScheduleId(int id)
        {
            var schedule = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.Id == id);
            if (schedule != null)
            {
                return schedule.Definition;

            }
            return "";
        }
        public async Task<int> GetSessionBranchIdBySessionIdandBranchId(int sessionId,int branchId)
        {
            var sessionBranch = await _uow.GetRepositry<AppSessionBranch>().FindByFilterAsync(x => x.SessionId == sessionId && x.BranchId == branchId);
            var sessionBranchıd = sessionBranch.Id;
            return sessionBranchıd;
        }
        public async Task<IResponse<AppScheduleCreateDto>> CreateScheduleAsync(AppScheduleCreateDto dto)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var schedule = _mapper.Map<AppSchedule>(dto);
                await _uow.GetRepositry<AppSchedule>().CreateAsync(schedule);
                await _uow.SaveChangesAsync();
                return new Response<AppScheduleCreateDto>(ResponseType.Success, dto);
            }
                return new Response<AppScheduleCreateDto>(dto, validationResult.ConvertToCustomValidationError());

        }
        public async Task<int> GetAppScheduleIdBySessionBranchIdAsync(int sessionBranchId)
        {
            var entity = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.SessionBranchId == sessionBranchId);
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<int> GetSessionIdBySessionBranchId(int sessionBranchId)
        {
            var entity = await _uow.GetRepositry<AppSession>().FindByFilterAsync(x => x.AppSessionBranches.Any(x => x.Id == sessionBranchId));
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<int> GetBranchIdBySessionBranchId(int sessionBranchId)
        {
            var entity = await _uow.GetRepositry<AppBranch>().FindByFilterAsync(x => x.AppSessionBranches.Any(x => x.Id == sessionBranchId));
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<int> GetSessionBranchIdByAppScheduleId(int scheduleId)
        {
            var entity = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.Id == scheduleId);
            if (entity != null)
            {
                return entity.SessionBranchId;

            }
            return 0;
        }
        public async Task<int> GetBranchIdByScheduleId(int scheduleId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.AppSessionBranches.Any(x => x.AppSchedules.Any(x => x.Id == scheduleId))).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Id;

            }
            return 0;
        }
        public async Task<string> GetBranchNameByScheduleId(int scheduleId)
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var entity = await query.Where(x => x.AppSessionBranches.Any(x => x.AppSchedules.Any(x => x.Id == scheduleId))).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Definition;

            }
            return "";
        }
        public async Task<int> GetScheduleIdByUserAndSessionId(int sessionId,int userId)
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entity = await query.Where(x => x.AppSessionBranch.SessionId == sessionId && x.AppSessionBranch.AppBranch.AppUsers.Any(x => x.Id == userId)).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Id;
            }
            return 0;
        }
        
        public async Task<List<int>> GetSCheduleIdsForTeacher(int userId,int sessionId)
        {
            List<int> scheduleIds = new List<int>();
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entities = await query.Where(x => x.AppScheduleDetails.Any(x => x.AppLesson.AppUser.Id == userId) && x.AppSessionBranch.SessionId == sessionId).ToListAsync();
            if (entities.Count != 0)
            {
                foreach( var entity in entities)
                {
                    scheduleIds.Add(entity.Id);
                }
                
            }
            return scheduleIds;

        }
       
        //public async Task<IResponse<AppScheduleCreateDto>> CreateScheduleWithBranchAsync(AppScheduleCreateDto dto, int branchId)
        //{
        //    var validationResult = _createDtoValidator.Validate(dto);
        //    if (validationResult.IsValid)
        //    {
        //        var schedule = _mapper.Map<AppSchedule>(dto);
        //       await _uow.GetRepositry<AppSchedule>().CreateAsync(schedule);
        //        await _uow.SaveChangesAsync();
        //        var data= await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.BranchId == branchId);

        //        //var query = _uow.GetRepositry<AppBranch>().GetQuery();
        //        //var entity = await query.SingleOrDefaultAsync(x => x.Id == branchId);
        //        ////entity.ScheduleId = data.Id;
        //        //await _uow.SaveChangesAsync();


        //        return new Response<AppScheduleCreateDto>(ResponseType.Success, dto);

        //    }
        //    return new Response<AppScheduleCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        //}
        //        public async Task<int> GetScheduleIdByBranchIdandSessionId(int branchId,int sessionId)
        //        {
        //            var query =  _uow.GetRepositry<AppSchedule>().GetQuery();
        //            var entity = await query.SingleOrDefaultAsync(x => x.BranchId == branchId && x.SessionId == sessionId);

        //            //var query = _uow.GetRepositry<AppSchedule>().GetQuery();
        //            //var entity = await query.SingleOrDefaultAsync(x => x.BranchId == branchId);
        //            return entity.Id;
        //        }
        //public async Task<int> GetBranchIdByScheduleId(int scheduleId)
        //{
        //    var entity = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.Id == scheduleId);

        //    return entity.AppSessionBranch.BranchId;
        //}
        public async Task<int> GetSessionIdByScheduleId(int scheduleId)
        {
            //var entity = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.Id == scheduleId);
            //return entity.AppSessionBranch.SessionId;
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entity = await query.Include(x => x.AppSessionBranch).Where(x => x.Id == scheduleId).SingleOrDefaultAsync();
            return entity.AppSessionBranch.SessionId;
        }
        
        public async Task<List<int>> GetLessonIdFromScheduleDetailsByScheduleId(int scheduleId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var scheduleDetails = await query.Where(x => x.ApScheduleId == scheduleId).ToListAsync();
            List<int> lessonIds = new();
            foreach (var item in scheduleDetails)
            {
                lessonIds.Add(item.LessonId);
            }
            var distinctLessonIds = lessonIds.Distinct().ToList();
            return distinctLessonIds;
        }
        public async Task<AppScheduleListDto> GetScheduleByScheduleId(int scheduleId)
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var schedule = await query.Where(x => x.Id == scheduleId).SingleOrDefaultAsync();
            if (schedule != null)
            {
                var mappedSchedule = _mapper.Map<AppScheduleListDto>(schedule);
                return mappedSchedule;
            }
            return new AppScheduleListDto();
            
           
        }
        public async Task RemoveScheduleByScheduleId(int scheduleId)
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entity = await query.Where(x => x.Id == scheduleId).SingleOrDefaultAsync();
            
            if (entity != null)
            {
                _uow.GetRepositry<AppSchedule>().Remove(entity);
               await  _uow.SaveChangesAsync();
            }
        }
        public async Task RemoveScheduleBySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppSchedule>().GetQuery();
            var entities = await query.Where(x => x.AppSessionBranch.SessionId == sessionId).ToListAsync();
            
            if (entities.Count != 0)
            {
                foreach(var item in entities)
                {
                    _uow.GetRepositry<AppSchedule>().Remove(item);

                }
                await _uow.SaveChangesAsync();
            }
        }


    }
}
