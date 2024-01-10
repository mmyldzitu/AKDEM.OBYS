using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppUserSessionService : IGenericService<AppUserSessionCreateDto, AppUserSessionUpdateDto, AppUserSessionListDto, AppUserSession>
    {
        Task CreateUserSessionAsync(List<AppUserSessionCreateDto> dtos);
        Task<List<int>> GetUserSessionsByBranchId(int sessionId,int branchId);
        Task<bool> CreateUserSessionByUserIdAsync(List<AppUserSessionCreateDto> dtos);
        Task<List<int>> GetUserSessionIdsByUserIdAsync(int userId);
        Task RemoveUserSessions(List<int> userSessionList);
        Task FindAverageOfSessionWithUserAndSession(int userId, int sessionId);
        Task TotalAverageByUserId(int userId,int sessionId);
        Task TotalAverageByUserIdGeneral(int userId);
        Task<int> UserSessionIdByUserIdAndSessionId(int userId, int sessionId);
        
        Task<int> GetSessionIdByUserSessionId(int userSessionId);
         Task<int> GetBranchIdByUserSessionId(int userSessionId);
        Task RemoveUserSessionBySessionId(int sessionId);
        Task RemoveUserSessionByUserId(int userId);
        Task<int> GetUserIdByUserSessionId(int userSessionId);
        Task<string> GetUserNameByUserSessionId(int userSessionId);
        Task<double> ReturnTotalAverage(int userId);
        Task<double> ReturnSessionAverage(int userSessionId);
        Task SetZeroToSessionAverage(int userId, int sessionId);
        Task TotalAverageAllUsers(int sessionId);
        Task TotalAverageAllUsersGeneral();
        Task SetZeroToSessionAverageBySessionId(int sessionId);
        Task<int> ReturnSessionOrderOfBranch(int userId, int branchId,int sessionId);
        Task<int> ReturnTotalOrderOfBranch(int userId, int branchId,int sessionId);
        Task<int> ReturnTotalOrderOfBranchGeneral(int userId, int branchId);
        Task<int> ReturnSessionOrderOfClass(int userId, int branchId,int sessionId);
        Task<int> ReturnTotalOrderOfClass(int userId, int branchId,int sessionId);
        Task<int> ReturnTotalOrderOfClassGeneral(int userId, int branchId);
        Task<int> GetClassIdByUserSessionId(int userSessionId);
        Task<bool>GetUserSessionStatus(int userSessionId);
        Task<string> ReturnPassiveDateByUserId(int userId);
        Task<string> LastBranchNameByUserAndSessionId(int sessionId, int userId);
    }
}
