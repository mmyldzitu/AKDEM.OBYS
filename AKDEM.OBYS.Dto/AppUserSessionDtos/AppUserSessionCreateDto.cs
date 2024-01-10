using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionDtos
{
    public class AppUserSessionCreateDto : IDto
    {
        public double Average { get; set; } = -1;
        public int SessionWarningCount { get; set; } = 0;
        
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public int BranchId { get; set; }
        public bool Status { get; set; } = true;
        public int SessionId { get; set; }
        
    }
}
