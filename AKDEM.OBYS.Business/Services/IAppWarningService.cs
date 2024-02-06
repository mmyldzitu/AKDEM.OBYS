using AKDEM.OBYS.Dto.AppWarningDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppWarningService : IGenericService<AppWarningCreateDto, AppWarningUpdateDto, AppWarningListDto, AppWarning>
    {
        Task RemoveWarningByUserId(int userId);
        Task CreateWarningByDtoandString(AppWarningCreateDto dto, string name, int userId, int userSessionId);
        Task  RemoveWarningByString(string lessonName, int userId, int usersessionId);
        Task<double> SessionWarningCountByUserSessionId(int userSessionId,double slwc);
        Task<double> TotalWarningCountByUserId(int userId,int sessionId);
        Task<double> TotalWarningCountByUserIdGeneral(int userId);
        Task<List<AppWarningListDto>> AppWarningByUserSessionId(int userSessionId);
        Task RemoveWarningById(int id, int userId, int userSessionId);
        Task ChangeStudentStatusBecasuseOfWarning(int userId, double sessionWarningCount, double absenteismWarningCount, double totalWarningCount, int userSessionId);
        Task<double> ReturnSwc(int userSessionId);
        Task<double> ReturnTwc(int userId);
        Task<int> FindWarningCountByString(string lessonName, int userSessionId);
        Task RemoveWarningByUserIdandSessionId(int userId, int sessionId,int userSessionId);
        Task SaveChangesAboutWarning(int userId, double sessionWarningCount, double totalWarningCount, int userSessionId);
        Task RemoveWarningBySessionId(int sessionId);
        Task<List<AppWarningListDto>> AppWarningBySessionId(int SessionId);
        Task<double> ReturnTwcGeneral(int userId);
        Task<List<AppWarningListDto>> AppWarningByUserId(int userId);
        Task<double> SessionLessonWarningCountByUserSessionId(int userSessionId);
        Task RemoveLessonWarnings(string mystring, int userId, int userSessionId);
        Task CreateAbsenteismWarningByDtoandString(AppWarningCreateDto dto, string name, int userId, int userSessionId);
        Task RemoveAbsenteismWarningByString(string lessonName, int userId, int userSessionId);
        Task<double> ReturnAwc(int userSessionId);
        Task<double> AbsenteismWarningCountByUserSessionId(int userSessionId);
       




    }
}
