using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppUserSession:BaseEntity
    {
        public double Average { get; set; }
        public double SessionWarningCount { get; set; }
        public double SessionLessonWarningCount { get; set; }
        public double SessionAbsentWarningCount { get; set; }
        public List<AppUserSessionLesson> AppUserSessionLessons { get; set; }
        public int UserId { get; set; }
        public AppUser AppUser { get; set; }
        public int SessionId { get; set; }
        public List<AppWarning> AppWarnings { get; set; }
        public AppSession AppSession { get; set; }
        public AppBranch AppBranch { get; set; }
        public int BranchId { get; set; }
        public int ClassId { get; set; }
        public bool Status { get; set; }

    }
}
