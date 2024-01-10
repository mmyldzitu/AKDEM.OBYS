using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppSessionBranchDtos
{
    public class AppSessionBranchListDto:IDto
    {
        public int SessionId { get; set; }
        public AppSessionListDto AppSession { get; set; }
        public int BranchId { get; set; }
        public AppBranchListDto AppBranch { get; set; }
        public int ScheduleId { get; set; }
        public AppScheduleListDto AppSchedule { get; set; }
    }
}
