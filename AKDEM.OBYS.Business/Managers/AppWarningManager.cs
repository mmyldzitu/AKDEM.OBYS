using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppWarningDtos;
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
    public class AppWarningManager : GenericManager<AppWarningCreateDto, AppWarningUpdateDto, AppWarningListDto, AppWarning>, IAppWarningService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IAppUserSessionService _appUserSessionService;
        private readonly IAppStudentService _appStudentService;
        public AppWarningManager(IMapper mapper, IValidator<AppWarningCreateDto> createDtoValidator, IValidator<AppWarningUpdateDto> updateDtoValidator, IUow uow, IAppUserSessionService appUserSessionService, IAppStudentService appStudentService) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
            _appUserSessionService = appUserSessionService;
            _appStudentService = appStudentService;
        }
        public async Task RemoveWarningByUserId(int userId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.UserId == userId);
            if (entities.Count != 0)
            {
                foreach (var item in entities)
                {
                    _uow.GetRepositry<AppWarning>().Remove(item);

                }
                
                await _uow.SaveChangesAsync();
                

            }
        }
        public async Task RemoveWarningBySessionId(int sessionId)
        {
            List<int> userSessionIds = new List<int>();
            List<int> userIds = new List<int>();
            
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.SessionId == sessionId);
            if (entities.Count != 0)
            {
                foreach( var entity in entities)
                {
                    userSessionIds.Add(entity.UserSessionId);
                    int userId = await _appUserSessionService.GetUserIdByUserSessionId(entity.UserSessionId);
                    userIds.Add(userId);
                }
               
                foreach(var entity in entities)
                {
                    _uow.GetRepositry<AppWarning>().Remove(entity);
                }
                for (int i = 0; i < userSessionIds.Count; i++)
                {
                    double swc = await SessionWarningCountByUserSessionId(userSessionIds[i]);
                    double twc = await TotalWarningCountByUserId(userIds[i],sessionId);
                    await ChangeStudentStatusBecasuseOfWarning(userIds[i], swc, twc, userSessionIds[i]);
                }
                await _uow.SaveChangesAsync();
            }
        }
        public async Task RemoveWarningByUserIdandSessionId(int userId,int sessionId,int userSessionId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.UserId == userId && x.AppUserSession.SessionId==sessionId);
            if (entities.Count != 0)
            {
                foreach (var item in entities)
                {
                    _uow.GetRepositry<AppWarning>().Remove(item);

                }

                await _uow.SaveChangesAsync();
                double swc = await SessionWarningCountByUserSessionId(userSessionId);
                double twc = await TotalWarningCountByUserId(userId,sessionId);
                await ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);


            }
        }
        public async Task CreateWarningByDtoandString(AppWarningCreateDto dto,string name,int userId, int userSessionId)
        {
            int sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            
            

            var control = await ControlIfLessonWarningAlreadyExists(name, userSessionId);

            if (!control)
            {
                var mappedEntity = _mapper.Map<AppWarning>(dto);
                await _uow.GetRepositry<AppWarning>().CreateAsync(mappedEntity);
                await _uow.SaveChangesAsync();
                double swc = await SessionWarningCountByUserSessionId(userSessionId);
                double twc = await TotalWarningCountByUserId(userId,sessionId);
                await ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);
            }

            
        }
        public async Task<bool> ControlIfLessonWarningAlreadyExists(string name,int userSessionId)
        {
            if(name.Contains("dönem ortalaması"))
            {

                var control2 = await _uow.GetRepositry<AppWarning>().FindByFilterAsync(x => x.UserSessionId == userSessionId && x.WarningReason.Contains(name));
                if (control2 == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {

                var control = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.UserSessionId == userSessionId);
                if (control.Count != 0)
                {
                    int sayac = 0;
                    foreach (var item in control)
                    {
                        string[] words = item.WarningReason.Split(' ');
                        for (int i = 0; i < words.Length; i++)
                        {
                            if (words[i] == "isimli" && i > 0)
                            {
                                string previousWord = words[i - 1];

                                if (previousWord == name)
                                {
                                    sayac += 1;
                                }
                                
                            }
                            
                        }

                    }
                    if (sayac == 0)
                    {
                        return false;
                    }
                    else { return true; };
                }
                return false;
            }
            
        }

        public async Task RemoveWarningByString(string lessonName,int userId,int userSessionId)
        {
            int sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.UserSessionId == userSessionId && x.WarningReason.Contains(lessonName));
            if (entities.Count != 0)
            {
                foreach( var entity in entities)
                {
                    _uow.GetRepositry<AppWarning>().Remove(entity);
                }

                await _uow.SaveChangesAsync();
                double swc = await SessionWarningCountByUserSessionId(userSessionId);
                double twc = await TotalWarningCountByUserId(userId,sessionId);
                await ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);
            }
        }
        public async Task<int> FindWarningCountByString(string lessonName, int userSessionId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.UserSessionId == userSessionId && x.WarningReason.Contains(lessonName));
            return entities.Count();
        }
        public async Task<double> SessionWarningCountByUserSessionId(int userSessionId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.UserSessionId==userSessionId);
            double sessionWarningCount = 0;
            if (entities.Count !=0)
            {
                foreach(var entity in entities)
                {
                    sessionWarningCount = sessionWarningCount + entity.WarningCount;
                }
                return sessionWarningCount;
            }
            return 0;
        }
        public async Task<double> TotalWarningCountByUserId(int userId,int sessionId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.UserId == userId && x.AppUserSession.SessionId<=sessionId);
            double totalWarningCount = 0;
            if (entities.Count != 0)
            {
                foreach (var entity in entities)
                {
                    totalWarningCount = totalWarningCount + entity.WarningCount;
                }
                return totalWarningCount;
            }
            return 0;
        }
        public async Task<double> TotalWarningCountByUserIdGeneral(int userId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.UserId == userId );
            double totalWarningCount = 0;
            if (entities.Count != 0)
            {
                foreach (var entity in entities)
                {
                    totalWarningCount = totalWarningCount + entity.WarningCount;
                }
                return totalWarningCount;
            }
            return 0;
        }
        public async Task<List<AppWarningListDto>> AppWarningByUserSessionId(int userSessionId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.UserSessionId == userSessionId);
            if (entities.Count != 0)
            {
                var descList = entities.OrderByDescending(x => x.Id).ToList();
                var mappedList = _mapper.Map<List<AppWarningListDto>>(descList);
                return mappedList;
            }
            return new List<AppWarningListDto>();
        }

        public async Task<List<AppWarningListDto>> AppWarningByUserId(int userId)
        {
            var entities = await _uow.GetRepositry<AppWarning>().GetAllAsync(x => x.AppUserSession.UserId== userId);
            if (entities.Count != 0)
            {
                var descList = entities.OrderByDescending(x => x.Id).ToList();
                var mappedList = _mapper.Map<List<AppWarningListDto>>(descList);
                return mappedList;
            }
            return new List<AppWarningListDto>();
        }

        public async Task<List<AppWarningListDto>> AppWarningBySessionId(int SessionId)
        {
            var query = _uow.GetRepositry<AppWarning>().GetQuery();
            var entities = await query.Include(x => x.AppUserSession).ThenInclude(x => x.AppUser).Where(x => x.AppUserSession.SessionId == SessionId).ToListAsync();
            
            if (entities.Count != 0)
            {
                var mappedList = _mapper.Map<List<AppWarningListDto>>(entities);
                var sortedList = mappedList.OrderByDescending(x => x.Id).ToList();
                return sortedList;
            }
            return new List<AppWarningListDto>();
        }
        public async Task RemoveWarningById(int id,int userId,int userSessionId)
        {
            int sessionId = await _appUserSessionService.GetSessionIdByUserSessionId(userSessionId);

            var entity = await _uow.GetRepositry<AppWarning>().FindAsync(id);
            _uow.GetRepositry<AppWarning>().Remove(entity);
            await _uow.SaveChangesAsync();
            double swc = await SessionWarningCountByUserSessionId(userSessionId);
            double twc = await TotalWarningCountByUserId(userId,sessionId);
            await ChangeStudentStatusBecasuseOfWarning(userId, swc, twc, userSessionId);

        }
        
        public async Task ChangeStudentStatusBecasuseOfWarning(int userId, double sessionWarningCount, double totalWarningCount, int userSessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var query2 = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            var entity2 = await query2.Where(x => x.Id == userSessionId).SingleOrDefaultAsync();
            if (entity != null && entity2 != null)
            {
                entity.TotalWarningCount = totalWarningCount;
                entity2.SessionWarningCount = sessionWarningCount;

                if (entity2.SessionWarningCount >= 2 )
                {
                    entity2.Status = false;
                    entity.Status = false;
                    

                }
                if (entity2.SessionWarningCount >= 2 || entity.TotalWarningCount >= 3)
                {
                    
                    entity.Status = false;
                    entity2.Status = false;

                }
                else
                {
                    entity2.Status = true;
                    entity.Status = true;
                    await _appStudentService.CreateStudentOrChangeStatusProcessForUserSessionandUSerSessionLessons(userId);
                }
                    

                
            }

            await _uow.SaveChangesAsync();

        }

        public async Task SaveChangesAboutWarning(int userId, double sessionWarningCount, double totalWarningCount, int userSessionId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var query2 = _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            var entity2 = await query2.Where(x => x.Id == userSessionId).SingleOrDefaultAsync();
            entity.TotalWarningCount = totalWarningCount;
            entity2.SessionWarningCount = sessionWarningCount;

            
            await _uow.SaveChangesAsync();

        }
        public async Task<double> ReturnSwc(int userSessionId)
        {
            var query =  _uow.GetRepositry<AppUserSession>().GetQuery();
            var entity = await query.Where(x => x.Id == userSessionId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.SessionWarningCount;

            }
            return 0;
        }
        public async Task<double> ReturnTwc(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.TotalWarningCount;

            }
            return 0;
        }
        public async Task<double> ReturnTwcGeneral(int userId)
        {
            await TotalWarningCountByUserIdGeneral(userId);
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                return entity.TotalWarningCount;

            }
            return 0;
        }

    }
}
