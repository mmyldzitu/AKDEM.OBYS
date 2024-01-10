using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppScheduleDtos
{
    public class AppScheduleCreateDto:IDto
    {
        public string Definition { get; set; }
        public int SessionBranchId { get; set; }

    }
}
