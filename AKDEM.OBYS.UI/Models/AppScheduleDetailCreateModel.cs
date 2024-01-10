using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppScheduleDetailCreateModel
    {
        public string Day { get; set; }
        public string FirstHour { get; set; }
        public string LastHour { get; set; }
        public int SessionId { get; set; }
        public int BranchId { get; set; }
        public int LessonId { get; set; }
        public string Definition { get; set; }


        public int ApScheduleId { get; set; }
    }
}
