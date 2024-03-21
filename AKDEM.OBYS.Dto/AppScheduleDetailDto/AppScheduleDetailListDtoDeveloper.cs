using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppScheduleDetailDto
{
    public class AppScheduleDetailListDtoDeveloper
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public string Hours { get; set; }
        public int LessonId { get; set; }
        public int ApScheduleId { get; set; }
    }
}
