using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppLessonDtos;
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
    public class AppUserSessionLessonManager : GenericManager<AppUserSessionLessonCreateDto, AppUserSessionLessonUpdateDto, AppUserSessionLessonListDto, AppUserSessionLesson>, IAppUserSessionLessonService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        
        public AppUserSessionLessonManager(IMapper mapper, IValidator<AppUserSessionLessonCreateDto> createDtoValidator, IValidator<AppUserSessionLessonUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
           
        }

        public async Task CreateUserSessionLessonAsync(List<AppUserSessionLessonCreateDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var control = await _uow.GetRepositry<AppUserSessionLesson>().FindByFilterAsync(x => x.UserSessionId == dto.UserSessionId && x.LessonId == dto.LessonId);
                if (control == null)
                {
                    var entity = _mapper.Map<AppUserSessionLesson>(dto);
                    await _uow.GetRepositry<AppUserSessionLesson>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();

                }
               
            }
        }
        public async Task RemoveUserSessionLessonByLessonNameAsync(int userSessionId, string lessonName)
        {
            
            var entity = await _uow.GetRepositry<AppUserSessionLesson>().FindByFilterAsync(x => x.AppLesson.Definition == lessonName && x.UserSessionId==userSessionId ,asNoTracking:true );
            if (entity != null)
            {
                _uow.GetRepositry<AppUserSessionLesson>().Remove(entity);
                await _uow.SaveChangesAsync();
            }
            
            
        }
        public async Task<bool> IsThereAnyScheduleForThisUser(int branchId)
        {
           
            var entities = await _uow.GetRepositry<AppScheduleDetail>().GetAllAsync(x => x.AppSchedule.AppSessionBranch.BranchId == branchId);
            if (entities != null)
            {
                return true;
            }
            else return false;

            
        }
        public async Task<int> GetScheduleIdForThisUser(int branchId)
        {
            var session = await _uow.GetRepositry<AppSession>().FindByFilterAsync(x => x.Status2 == true);
            if (session != null)
            {
                var sessionBranch = await _uow.GetRepositry<AppSessionBranch>().FindByFilterAsync(x => x.SessionId == session.Id && x.BranchId == branchId);
                if (sessionBranch != null)
                {
                    var entity = await _uow.GetRepositry<AppSchedule>().FindByFilterAsync(x => x.AppSessionBranch.Id == sessionBranch.Id);
                    if (entity != null)
                    {
                        return entity.Id;
                    }
                    return 0;
                }


               
                else return 0;
            }
            
            else return 0;
        }
        public async Task RemoveUserSessionLessonsByUserSessionId(int userSessionId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entities = await query.Where(x => x.AppUserSession.Id == userSessionId).ToListAsync();
            if (entities.Count != 0)
            {
                foreach(var entity in entities)
                {
                    _uow.GetRepositry<AppUserSessionLesson>().Remove(entity);

                }
            }
            await _uow.SaveChangesAsync();
        }
        public async Task CreateUserSessionLessonAsyncByUserSessions(List<AppUserSessionLessonCreateDto> dtos)
        {
           foreach ( var dto in dtos)
            {
                var controlentity = await _uow.GetRepositry<AppUserSessionLesson>().FindByFilterAsync(x => x.LessonId == dto.LessonId && x.UserSessionId == dto.UserSessionId);
                if (controlentity == null)
                {
                    var entity = _mapper.Map<AppUserSessionLesson>(dto);
                    await _uow.GetRepositry<AppUserSessionLesson>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                }
                

            }
        }
        public async Task RemoveUserSessionLessonsByUserSessionListAsync(List<int> dtos)
        {
            if (dtos.Count != 0)
            {
                foreach (var userSession in dtos)
                {
                    var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
                    var entities = await query.Where(x => x.UserSessionId == userSession).ToListAsync();
                    
                    if (entities.Count!=0)
                    {
                        foreach( var entity in entities)
                        {
                            _uow.GetRepositry<AppUserSessionLesson>().Remove(entity);
                            await _uow.SaveChangesAsync();
                        }
                        
                    }

                }
            }
           
        }
        public async Task<List<AppUserSessionLessonUpdateDto>> GetAppUserSessionLessonsByUserSessionId(int userSessionId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x=>x.AppUser).Where(x => x.UserSessionId == userSessionId).ToListAsync();
             var lessonquery = _uow.GetRepositry<AppLesson>().GetQuery();

            List<AppUserSessionLessonUpdateDto> dtoList = new List<AppUserSessionLessonUpdateDto>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    string mystatus = "0";
                    if (item.Not == -5)
                    {
                        mystatus = "1";
                    }
                    else if(item.Not == -6)
                    {
                        mystatus = "2";
                    }
                    else
                    {
                        mystatus= "0";
                    }

                    var appLessonEntity = await lessonquery.Include(x=>x.AppUser).Where(x=>x.Id==item.LessonId).SingleOrDefaultAsync();
                    var appLesson = _mapper.Map<AppLessonListDto>(appLessonEntity);
                    
                    dtoList.Add(new AppUserSessionLessonUpdateDto { Id = item.Id, LessonId = item.LessonId, AppLesson = appLesson, UserSessionId = item.UserSessionId, Not = item.Not, Devamsızlık = item.Devamsızlık, Status = mystatus });


                }

                return dtoList;
            }
            return new List<AppUserSessionLessonUpdateDto>();
        }
        public async Task<List<AppUserSessionLessonUpdateDto>> GetAppUserSessionLessonsByUserSessionIdAndLessonId(int userSessionId,int lessonId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entities = await query.Include(x => x.AppLesson).ThenInclude(x => x.AppUser).Where(x => x.UserSessionId == userSessionId && x.LessonId==lessonId).ToListAsync();
            var lessonquery = _uow.GetRepositry<AppLesson>().GetQuery();

            List<AppUserSessionLessonUpdateDto> dtoList = new List<AppUserSessionLessonUpdateDto>();
            if (entities != null)
            {
                foreach (var item in entities)
                {
                    string mystatus = "0";
                    if (item.Not == -5)
                    {
                        mystatus = "1";
                    }
                    else if (item.Not == -6)
                    {
                        mystatus = "2";
                    }
                    else
                    {
                        mystatus = "0";
                    }

                    var appLessonEntity = await lessonquery.Include(x => x.AppUser).Where(x => x.Id == item.LessonId).SingleOrDefaultAsync();
                    var appLesson = _mapper.Map<AppLessonListDto>(appLessonEntity);

                    dtoList.Add(new AppUserSessionLessonUpdateDto { Id = item.Id, LessonId = item.LessonId, AppLesson = appLesson, UserSessionId = item.UserSessionId, Not = item.Not, Devamsızlık = item.Devamsızlık, Status = mystatus });


                }

                return dtoList;
            }
            return  new List<AppUserSessionLessonUpdateDto>();
        }
        public async Task UpdateUserSessionLessonsAsync(List<AppUserSessionLessonUpdateDto> dtos)
        {
            foreach ( var dto in dtos)
            {
                var entity = await _uow.GetRepositry<AppUserSessionLesson>().FindByFilterAsync(x => x.Id == dto.Id);
                if(entity != null)
                {
                    entity.Not = dto.Not;
                    entity.Devamsızlık = dto.Devamsızlık;
                    
                    await _uow.SaveChangesAsync();
                }
            }
        }
        public async Task RemoveUserSessionLessonBySessionId (int sessionId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entities = await query.Where(x => x.AppUserSession.AppSession.Id == sessionId).ToListAsync();
            
            if (entities.Count != 0)
            {
                foreach(var item in entities)
                {
                    _uow.GetRepositry<AppUserSessionLesson>().Remove(item);
                }
                await _uow.SaveChangesAsync();
            }
        }
        public async Task<List<AppUserSessionLessonListDto>> UserSessionLessonDetailsBySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entities = await query.Include(x => x.AppUserSession).ThenInclude(x => x.AppUser).Include(x => x.AppUserSession).ThenInclude(x => x.AppBranch).Include(x => x.AppLesson).ThenInclude(x => x.AppUser).Where(x => x.AppUserSession.SessionId == sessionId && x.Not == -1).ToListAsync();
            if (entities.Count != 0)
            {
                return _mapper.Map<List<AppUserSessionLessonListDto>>(entities);
            }
            return new List<AppUserSessionLessonListDto>();
        }

        public async Task<double> GetLessonNoteByUserSessionIdAndLessonId(int userSessionId,int lessonId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entity = await query.Where(x => x.UserSessionId == userSessionId && x.LessonId == lessonId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Not;
            }
            return -1;
        }
        public async Task<int> GetLessonDevamsByUserSessionIdAndLessonId(int userSessionId, int lessonId)
        {
            var query = _uow.GetRepositry<AppUserSessionLesson>().GetQuery();
            var entity = await query.Where(x => x.UserSessionId == userSessionId && x.LessonId == lessonId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Devamsızlık;
            }
            return -1;
        }
    }
}
