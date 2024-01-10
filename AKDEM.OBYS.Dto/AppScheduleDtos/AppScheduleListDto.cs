using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppScheduleDtos
{
    public class AppScheduleListDto:IDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int SessionBranchId { get; set; }
        public AppSessionBranchListDto AppSessionBranch { get; set; }
        
    }
}
