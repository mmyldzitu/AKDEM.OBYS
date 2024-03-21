using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionDtos
{
    public class AppUserSessionCreateDtoDeveloper
    {

        public double Average { get; set; } = -1;
        public double SessionWarningCount { get; set; } = 0;
        public double SessionLessonWarningCount { get; set; } = 0;
        public double SessionAbsentWarningCount { get; set; } = 0;
        public int UserId { get; set; }

        public int SessionId { get; set; }
        public int BranchId { get; set; }
        public int ClassId { get; set; }
        public bool Status { get; set; } = true;
    }
}
