using AKDEM.OBYS.Common;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppUserSessionLessonService : IGenericService<AppUserSessionLessonCreateDto, AppUserSessionLessonUpdateDto, AppUserSessionLessonListDto, AppUserSessionLesson>
    {
        Task CreateUserSessionLessonAsync(List<AppUserSessionLessonCreateDto> dtos);
        Task RemoveUserSessionLessonByLessonNameAsync(int userSessionId, string lessonName);
        Task<bool> IsThereAnyScheduleForThisUser(int branchId);
        Task<int> GetScheduleIdForThisUser(int branchId);
        Task CreateUserSessionLessonAsyncByUserSessions(List<AppUserSessionLessonCreateDto> dtos);
        Task RemoveUserSessionLessonsByUserSessionListAsync(List<int> dtos);
        Task<List<AppUserSessionLessonUpdateDto>> GetAppUserSessionLessonsByUserSessionId(int userSessionId);
        Task UpdateUserSessionLessonsAsync(List<AppUserSessionLessonUpdateDto> dtos);
        Task RemoveUserSessionLessonBySessionId(int sessionId);
        Task<List<AppUserSessionLessonListDto>> UserSessionLessonDetailsBySessionId(int sessionId);
        Task<double> GetLessonNoteByUserSessionIdAndLessonId(int userSessionId, int lessonId);
        Task<int> GetLessonDevamsByUserSessionIdAndLessonId(int userSessionId, int lessonId);
        Task<List<AppUserSessionLessonUpdateDto>> GetAppUserSessionLessonsByUserSessionIdAndLessonId(int userSessionId, int lessonId);
        Task RemoveUserSessionLessonsByUserSessionId(int userSessionId);
        Task<List<AppUserSessionLessonListDtoDeveloper>> GetAppUserSessionLessonsDeveloper();
        Task CreateUserSessionLessonDeveloper(AppUserSessionLessonCreateDto dto);
        Task UpdateUserSessionLessonDeveloper(AppUserSessionLessonUpdateDtoDeveloper dto);
        Task<AppUserSessionLessonListDtoDeveloper> GetAppUserSessionLessonDeveloper(int id);
        Task RemoveAppUserSessionLessonDeveloper(int id);
    }
}
