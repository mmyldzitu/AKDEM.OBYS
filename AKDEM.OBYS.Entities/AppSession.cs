using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppSession:BaseEntity
    {
        public string Definition { get; set; }
        public double MinAverageNote { get; set; }
        public double MinLessonNote { get; set; }
        public int MinAbsenteeism { get; set; }
        public bool Status { get; set; }
        public bool Status2 { get; set; }
        public string SessionPresident { get; set; }
        public List<AppUserSession> AppUserSessions { get; set; }
        public List<AppSessionBranch> AppSessionBranches { get; set; }
    }
}
