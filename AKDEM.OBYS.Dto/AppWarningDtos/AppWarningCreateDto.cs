using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppWarningDtos
{
    public class AppWarningCreateDto:IDto 
    {
        public double WarningCount { get; set; } = 0;
        public DateTime WarningTime { get; set; }
        public string WarningReason { get; set; }

        public int UserSessionId { get; set; }
        
    }
}
