using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppWarning:BaseEntity
    {
        public int WarningCount { get; set; }
        public DateTime WarningTime { get; set; }
        public string WarningReason { get; set; }

        public int UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
