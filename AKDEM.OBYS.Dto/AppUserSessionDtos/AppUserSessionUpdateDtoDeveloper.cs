using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionDtos
{
    public class AppUserSessionUpdateDtoDeveloper
    {
        public int Id { get; set; }
        public double Average { get; set; }
        public double SessionWarningCount { get; set; }
        public double SessionLessonWarningCount { get; set; }
        public double SessionAbsentWarningCount { get; set; }
        public int UserId { get; set; }

        public int SessionId { get; set; }
        public int BranchId { get; set; }
        public int ClassId { get; set; }
        public bool Status { get; set; }
    }
}
