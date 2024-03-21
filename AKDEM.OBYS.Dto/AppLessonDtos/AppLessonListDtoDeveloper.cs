using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppLessonDtos
{
    public class AppLessonListDtoDeveloper
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
    }
}
