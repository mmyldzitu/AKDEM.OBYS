using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppScheduleDetailService : IGenericService<AppScheduleDetailCreateDto, AppScheduleDetailUpdateDto, AppScheduleDetailListDto, AppScheduleDetail>
    {
        Task<List<string>> GetHoursByScheduleIdAsync(int scheduleId);
        Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleId(int scheduldeId);
        Task RemoveByNameAsync(string name);
        Task<int> GetScheduleByIdScheduleDetailIdAsync(int scheduleDetailId);
        Task<string> GetLessonNameByScheduleDetailIdAsync(int scheduleDetailId);
        Task RemoveScheduleDetailByLessonNameScheduleId(int scheduleId, string lessonName);
        Task<List<int>> GetLessonsByScheduleIdAsync(int scheduleId);
        Task RemoveScheduleDetailsByScheduleId(int scheduleId);
        Task RemoveScheduleDetailBySessionId(int sessionId);
        Task<List<AppScheduleDetailListDto>> GetScheduleDetailsForTeacher(int sessionId, int userId);
        Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleIdDistinct(int scheduldeId, int userId);
        Task<List<AppScheduleDetailListDto>> GetScheduleDetailsByScheduleIdForTeacher(int scheduldeId, int userId);
    }
}
