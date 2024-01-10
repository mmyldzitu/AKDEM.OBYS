using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppSessionBranchDtos
{
    public class AppSessionBranchCreateDto : IDto
    {
        public int SessionId { get; set; }

        public int BranchId { get; set; }

        
        
    }
}
