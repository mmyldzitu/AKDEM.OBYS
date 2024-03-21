using AKDEM.OBYS.Common;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppScheduleService : IGenericService<AppScheduleCreateDto, AppScheduleUpdateDto, AppScheduleListDto, AppSchedule>
    {
        Task<List<AppScheduleListDto>> GetSchedulesBySession(int id);
        //Task<IResponse<AppScheduleCreateDto>> CreateScheduleWithBranchAsync(AppScheduleCreateDto dto, int branchId);
        //Task<int> GetScheduleIdByBranchIdandSessionId(int branchId, int sessionId);
        Task<int> GetSessionIdByScheduleId(int scheduleId);
        Task<int> GetBranchIdBySessionBranchId(int sessionBranchId);
        Task<string> GetNameByScheduleId(int id);
        //Task<int> GetBranchIdByScheduleId(int scheduleId);
        Task<List<int>> GetLessonIdFromScheduleDetailsByScheduleId(int scheduleId);
        Task<AppScheduleListDto> GetScheduleByScheduleId(int scheduleId);
        Task<int> GetSessionBranchIdBySessionIdandBranchId(int sessionId, int branchId);
        Task<int> GetAppScheduleIdBySessionBranchIdAsync(int sessionBranchId);
        Task<int> GetSessionIdBySessionBranchId(int sessionBranchId);
        Task<int> GetSessionBranchIdByAppScheduleId(int scheduleId);
        Task<int> GetBranchIdByScheduleId(int scheduleId);
        Task<IResponse<AppScheduleCreateDto>> CreateScheduleAsync(AppScheduleCreateDto dto);
        Task RemoveScheduleByScheduleId(int scheduleId);
         Task RemoveScheduleBySessionId(int sessionId);
        Task<List<int>> GetSCheduleIdsForTeacher(int userId, int sessionId);
        Task<string> GetBranchNameByScheduleId(int scheduleId);
        Task<int> GetScheduleIdByUserAndSessionId(int sessionId, int userId);
        Task<List<AppScheduleListDtoDeveloper>> GetAppSchedulesDeveloper();




    }
}
