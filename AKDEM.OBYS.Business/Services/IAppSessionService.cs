using AKDEM.OBYS.Common;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
   public interface IAppSessionService:IGenericService<AppSessionCreateDto,AppSessionUpdateDto,AppSessionListDto,AppSession>
    {
        Task SetStatusAsync(int sessionId);
        Task<IResponse<List<AppSessionListDto>>> GetOrderingAsync();
        Task<int> GetSeesionIdBySessionDefinition(string sessionDefinition);
        Task ChangeStatus2(string definition);
         Task<bool> GetStatus2FromSessionId(int sessionId);
         Task<bool> GetStatusFromSessionId(int sessionId);
        Task ChangeStatus2BySessionId(int sessionId);
        Task<string> ReturnSessionName(int sessionId);
        Task<List<AppSessionListDto>> GetSessionsByUserId(int userId);
        Task SetStatusAllexceptThis(int sessionId_2,int sessionId = -1);
        Task<int> Status2Count();
        Task<int> GetActiveSessionId();
        Task<List<AppSessionListDto>> TeacherExSessionsAsync(int userId);
        Task<List<AppSessionListDto>> StudentExSessionsAsync(int userId);
        Task<bool> IsThereSession(string sessionName);
        Task<double> MinLessonNoteOfSession(int sessionId);
        Task<double> MinAverageNoteOfSession(int sessionId);
        Task<int> MinAbsenteismOfSession(int sessionId);
        Task<int> PreviousSessionOfUser(int userSessionId, int sessionId);
        Task<AppSessionListDto> SessionCriterias(int sessionId);
        Task<AppSessionUpdateDto> UpdateSessionCriterias(int sessionId);
        Task UpdateSessionCriteriasPost(AppSessionUpdateDto dto);
        Task<bool> IfLessonAlreadyExists(string definition, int userId);
        Task RemoveUserSessionsBecauseOfBeingPassive(int sessionId, int userId);
        Task<string> ReturnPresidentName(int sessionId);
        Task RemoveSession(int sessionId);
    }
}
