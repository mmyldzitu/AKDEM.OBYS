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
    public interface IAppScheduleService:IGenericService<AppScheduleCreateDto,AppScheduleUpdateDto,AppScheduleListDto,AppSchedule>
    {
        Task<IResponse<List<AppScheduleListDto>>> GetSchedulesBySessionId(int id);
    }
}
