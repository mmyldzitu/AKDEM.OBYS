using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppScheduleDetailsForJsonModel
    {
        public List<string> ScheduleHours { get; set; }
        public List<AppScheduleDetailsTeacherModel> AppScheduleDetails { get; set; }
    }
}
