using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
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
    public class AppScheduleDetailManager : GenericManager<AppScheduleDetailCreateDto, AppScheduleDetailUpdateDto, AppScheduleDetailListDto, AppScheduleDetail>, IAppScheduleDetailService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public AppScheduleDetailManager(IMapper mapper, IValidator<AppScheduleDetailCreateDto> createDtoValidator, IValidator<AppScheduleDetailUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<string>> GetHoursByScheduleIdAsync(int scheduleId)
        {
            List<string> hours = new();
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Where(x => x.ApScheduleId == scheduleId).ToListAsync();
            foreach(var item in entities)
            {
                hours.Add(item.Hours);
            }
            return hours;
        }
        public async Task<List<AppScheduleDetailListDto>> GetScheduleDetailsForTeacher(int sessionId, int userId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x => x.AppUser).Include(x => x.AppSchedule).ThenInclude(x=>x.AppSessionBranch).ThenInclude(x=>x.AppBranch).Where(x => x.AppLesson.UserId == userId && x.AppSchedule.AppSessionBranch.SessionId == sessionId).ToListAsync();
            if (entities.Count != 0)
            {
                var mappedLists = _mapper.Map<List<AppScheduleDetailListDto>>(entities);
                return mappedLists;
            }
            return new List<AppScheduleDetailListDto>();
        }
        public async Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleId(int scheduldeId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x=>x.AppUser).Where(x => x.ApScheduleId == scheduldeId).ToListAsync();
            if (entities.Count != 0)
            {
                var mappedList = _mapper.Map<List<AppScheduleDetailListDto>>(entities);
                return mappedList;
            }
            return new List<AppScheduleDetailListDto>();
        }

        public async Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleIdForTeacher(int scheduldeId,int userId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x => x.AppUser).Where(x => x.ApScheduleId == scheduldeId && x.AppLesson.AppUser.Id==userId).ToListAsync();
            if (entities.Count != 0)
            {
                var mappedList = _mapper.Map<List<AppScheduleDetailListDto>>(entities);
                return mappedList;
            }
            return new List<AppScheduleDetailListDto>();
        }
        public async Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleIdDistinct(int scheduldeId, int userId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x => x.AppUser).Where(x => x.ApScheduleId == scheduldeId && x.AppLesson.AppUser.Id==userId).ToListAsync();
            if (entities.Count != 0)
            {
                var distinctEntities = entities
    .GroupBy(x => new { x.AppLesson.Definition })
    .Select(group => group.First())
    .ToList();

                var mappedList = _mapper.Map<List<AppScheduleDetailListDto>>(distinctEntities);
                return mappedList;
            }
            return new List<AppScheduleDetailListDto>();
            
        }

        public async Task RemoveByNameAsync(string name)
        {
            var data = await _uow.GetRepositry<AppScheduleDetail>().GetAllAsync(x => x.AppLesson.Definition == name);
            foreach (var item in data)
            {
                 _uow.GetRepositry<AppScheduleDetail>().Remove(item);
            }
            await _uow.SaveChangesAsync();
        }
      public async Task<int> GetScheduleByIdScheduleDetailIdAsync(int scheduleDetailId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entity = await query.Where(x => x.Id == scheduleDetailId).SingleOrDefaultAsync();
            var scheduleId = entity.ApScheduleId;
            return scheduleId;
        }
        public async Task<string> GetLessonNameByScheduleDetailIdAsync(int scheduleDetailId)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entity = await query.Include(x => x.AppLesson).Where(x => x.Id == scheduleDetailId).SingleOrDefaultAsync();
            var lessonName = entity.AppLesson.Definition;
            return lessonName;
        }

        public async Task RemoveScheduleDetailByLessonNameScheduleId(int scheduleId, string lessonName)
        {
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();

            var scheduleDetail = await query.Where(x => x.ApScheduleId == scheduleId && x.AppLesson.Definition == lessonName).ToListAsync();
            foreach(var item in scheduleDetail)
            {
                _uow.GetRepositry<AppScheduleDetail>().Remove(item);

                await _uow.SaveChangesAsync();

            }
        }
        public async Task<List<int>> GetLessonsByScheduleIdAsync(int scheduleId)
        {
            List<int> lessonIds = new ();
            var query = _uow.GetRepositry<AppScheduleDetail>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).Where(x => x.ApScheduleId == scheduleId).ToListAsync();
            foreach( var entity in entities)
            {
                lessonIds.Add(entity.AppLesson.Id);
            }
            return lessonIds;
        }
        public async Task RemoveScheduleDetailsByScheduleId(int scheduleId)
        {
            var entities = await _uow.GetRepositry<AppScheduleDetail>().GetAllAsync(x => x.ApScheduleId == scheduleId);
            if (entities.Count != 0)
            {
                foreach( var entity in entities)
                {
                    _uow.GetRepositry<AppScheduleDetail>().Remove(entity);
                    await _uow.SaveChangesAsync();
                }
            }
        }
        public async Task RemoveScheduleDetailBySessionId(int sessionId)
        {
            var entities = await _uow.GetRepositry<AppScheduleDetail>().GetAllAsync(x => x.AppSchedule.AppSessionBranch.SessionId == sessionId);
            if (entities.Count != 0)
            {
                foreach( var item in entities)
                {
                    _uow.GetRepositry<AppScheduleDetail>().Remove(item);
                }
                await _uow.SaveChangesAsync();
            }
        }
    }
}
