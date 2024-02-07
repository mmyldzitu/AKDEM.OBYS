using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppSessionDtos;
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
    public class AppSessionManager : GenericManager<AppSessionCreateDto, AppSessionUpdateDto, AppSessionListDto, AppSession>, IAppSessionService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IAppUserSessionService _appUserSessionService;


        public AppSessionManager(IMapper mapper, IValidator<AppSessionCreateDto> createDtoValidator, IValidator<AppSessionUpdateDto> updateDtoValidator, IUow uow/* IAppUserSessionService appUserSessionService, IAppWarningService appWarningService*/, IAppUserSessionService appUserSessionService) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
            _appUserSessionService = appUserSessionService;
        }
        public async Task<int> GetActiveSessionId()
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Status2 == true).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Id;
            }
            return 0;
        }
        public async Task RemoveSession(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (session != null)
            {
                _uow.GetRepositry<AppSession>().Remove(session);
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<bool> IfLessonAlreadyExists(string definition, int userId)
        {
            var query = _uow.GetRepositry<AppLesson>().GetQuery();
            var lessons = await query.Where(x => x.Definition == definition).ToListAsync();
            if (lessons.Count != 0)
            {
                foreach (var lesson in lessons)
                {
                    if (lesson.UserId == userId)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public async Task<string> ReturnPresidentName(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (session != null)
            {
                return session.SessionPresident;
            }
            return "";
        }
        public async Task SetStatusAsync(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.SingleOrDefaultAsync(x => x.Id == sessionId);
            if (entity.Status == true)
            {
                entity.Status = false;

            }
            else
            {
                entity.Status = true;
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<double> MinLessonNoteOfSession(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.MinLessonNote;
            }
            return 0;
        }
        public async Task<double> MinAverageNoteOfSession(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.MinAverageNote;
            }
            return 0;
        }
        public async Task<int> MinAbsenteismOfSession(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.MinAbsenteeism;
            }
            return 0;
        }
        public async Task RemoveUserSessionsBecauseOfBeingPassive(int sessionId,int userId)
        {
            var control = await GetStatus2FromSessionId(sessionId);
            if (!control)
            {
                int activeSessionId = await GetActiveSessionId();
                await _appUserSessionService.RemoveUserSessionsAndLessons(activeSessionId, userId);
            }
        }

        public async Task<int> PreviousSessionOfUser(int userSessionId,int sessionId)
        {
            
            var sessionName = await ReturnSessionName(sessionId);
            int userId = await _appUserSessionService.GetUserIdByUserSessionId(userSessionId);
            string[] parcalar = sessionName.Split(new char[] { '/' });
            string exsessionName = "";
            string yearName = parcalar[0];
            
            if (sessionName.Contains("Bahar"))
            {
                exsessionName = yearName + "/" + "Güz";
            }
            else if (sessionName.Contains("Yaz"))
            {
                exsessionName = yearName + "/" + "Bahar";
            }
            int exsessionId = await GetSeesionIdBySessionDefinition(exsessionName);
            int exUserSessionId = await _appUserSessionService.UserSessionIdByUserIdAndSessionId(userId, exsessionId);

            return exUserSessionId;
        }

        public async Task SetStatusAllexceptThis(int sessionId_2,int sessionId=-1)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Id == sessionId_2).SingleOrDefaultAsync();
            if (entity != null)
            {
                entity.Status2 = false;
                await _uow.SaveChangesAsync();
            }
            var entities = await query.Where(x => x.Id != sessionId).ToListAsync();
            if (entities.Count != 0)
            {
                foreach( var item in entities)
                {
                    item.Status = false;
                    await _uow.SaveChangesAsync();
                }
            }
        }
        public async Task<IResponse<List<AppSessionListDto>>> GetOrderingAsync()
        {
            var data = await _uow.GetRepositry<AppSession>().GetAllAsync( x => x.Id, Common.Enums.OrderByType.DESC);
            if (data.Count != 0)
            {
                var dto = _mapper.Map<List<AppSessionListDto>>(data);
                return new Response<List<AppSessionListDto>>(ResponseType.Success, dto);
            }
            return new Response<List<AppSessionListDto>>(ResponseType.NotFound, "İlgili Veri Bulunamadı");

        }
        public async Task<int> GetSeesionIdBySessionDefinition(string sessionDefinition)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Definition == sessionDefinition).SingleOrDefaultAsync();
            if (session != null)
            {
                var sessionId = session.Id;
                return sessionId;
            }
            return 0;
        }
        public async Task<int> Status2Count()
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entities = await query.Where(x => x.Status2 == true).ToListAsync();
            if(entities.Count != 0)
            {
                return entities.Count;
            }
            return 0;
        }
        public async Task ChangeStatus2 (string definition)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.SingleOrDefaultAsync(x => x.Definition == definition);
            if (entity != null)
            {
                var entities = await query.Where(x => x.Id != entity.Id).ToListAsync();
                if (entities != null)
                {
                    foreach (var item in entities)
                    {
                        if (item.Status2 == true)
                        {
                            item.Status2 = false;


                        }

                    }
                    await _uow.SaveChangesAsync();


                }

            }
            
        }
        public async Task ChangeStatus2BySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.SingleOrDefaultAsync(x => x.Id==sessionId);
            
            if (entity != null)
            {
                entity.Status2 = true;
                await _uow.SaveChangesAsync();
                var entities = await query.Where(x => x.Id != entity.Id).ToListAsync();
                if (entities != null)
                {
                    foreach (var item in entities)
                    {
                        if (item.Status2 == true)
                        {
                            item.Status2 = false;


                        }

                    }
                    await _uow.SaveChangesAsync();


                }

            }
        }
        public async Task<bool> GetStatus2FromSessionId(int sessionId)
        {
            var entity = await _uow.GetRepositry<AppSession>().FindAsync(sessionId);
            if (entity != null)
            {
                return entity.Status2;

            }
            return false;
        }
        public async Task<bool> GetStatusFromSessionId(int sessionId)
        {
            var entity = await _uow.GetRepositry<AppSession>().FindAsync(sessionId);
            if (entity != null)
            {
                return entity.Status;

            }
            return false;
        }
        public async Task<string> ReturnSessionName(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.Definition;

            }
            return "";
        }
        public async Task<List<AppSessionListDto>> GetSessionsByUserId(int userId)
        {
            var entities = await _uow.GetRepositry<AppSession>().GetAllAsync(x => x.Id,x => x.AppUserSessions.Any(x => x.UserId == userId), Common.Enums.OrderByType.DESC);
            if (entities.Count != 0)
            {
                var mappedList = _mapper.Map<List<AppSessionListDto>>(entities);
                return mappedList;
            }
            return new List<AppSessionListDto>();
        }
        public async Task<List<AppSessionListDto>> TeacherExSessionsAsync(int userId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entities = await query.Where( x =>x.Status2==false &&x.AppUserSessions.Any(x => x.AppUserSessionLessons.Any(x => x.AppLesson.UserId == userId))).ToListAsync();
            if (entities.Count != 0)
            {
                var descList = entities.OrderByDescending(x => x.Id).ToList();
                var mappedList = _mapper.Map<List<AppSessionListDto>>(descList);
                return mappedList;
            }
            return new List<AppSessionListDto>();
        }
        public async Task<List<AppSessionListDto>> StudentExSessionsAsync(int userId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entities = await query.Where(x => x.Status2 == false && x.AppUserSessions.Any(x=>x.AppUser.Id==userId)).ToListAsync();
            if (entities.Count != 0)
            {
                var descList = entities.OrderByDescending(x => x.Id).ToList();
                var mappedList = _mapper.Map<List<AppSessionListDto>>(descList);
                return mappedList;
            }
            return new List<AppSessionListDto>();
        }
        public async Task<bool> IsThereSession(string sessionName)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Definition == sessionName).SingleOrDefaultAsync();
            if (session == null)
            {
                return true;
            }
            else { return false; }
        }
        public async Task<AppSessionListDto> SessionCriterias (int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (session != null)
            {
                var mappedSession = _mapper.Map<AppSessionListDto>(session);
                return mappedSession;
            }
            return new AppSessionListDto();
        }
        public async Task<AppSessionUpdateDto> UpdateSessionCriterias(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Id == sessionId).SingleOrDefaultAsync();
            if (session != null)
            {
                var mappedSession = _mapper.Map<AppSessionUpdateDto>(session);
                return mappedSession;
            }
            return new AppSessionUpdateDto();
        }
        public async Task UpdateSessionCriteriasPost(AppSessionUpdateDto dto)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var session = await query.Where(x => x.Id == dto.Id).SingleOrDefaultAsync();
            if (session != null)
            {
                session.Status = dto.Status;
                session.Status2 = dto.Status2;
                session.Definition = dto.Definition;
                session.MinAbsenteeism = dto.MinAbsenteeism;
                session.MinAverageNote = dto.MinAverageNote;
                session.MinLessonNote = dto.MinLessonNote;
                session.SessionPresident = dto.SessionPresident;
            }
            await _uow.SaveChangesAsync();
        }
    }
}
