using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppScheduleCreateModel
    {
        public string Definition { get; set; }
        public int SessionId { get; set; }
        public int BranchId { get; set; }
    }
}
