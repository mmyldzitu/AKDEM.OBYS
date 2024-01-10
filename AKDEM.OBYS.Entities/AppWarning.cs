using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppWarning:BaseEntity
    {
        public double WarningCount { get; set; }
        public DateTime WarningTime { get; set; }
        public string WarningReason { get; set; }

        public int UserSessionId { get; set; }
        public AppUserSession AppUserSession { get; set; }
    }
}
