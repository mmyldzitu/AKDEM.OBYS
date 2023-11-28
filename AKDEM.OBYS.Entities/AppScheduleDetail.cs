using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppScheduleDetail:BaseEntity
    {
        public string Day { get; set; }
        public string Hours { get; set; }
        public int LessonId { get; set; }
        public AppLesson AppLesson { get; set; }
        public int ApScheduleId { get; set; }
        public AppSchedule AppSchedule { get; set; }

    }
}
