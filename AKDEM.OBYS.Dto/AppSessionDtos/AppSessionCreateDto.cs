using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppSessionDtos
{
   public class AppSessionCreateDto : IDto
    {
        public string Definition { get; set; }
        public bool Status { get; set; } = true;
        public bool Status2 { get; set; } = true;
        public double MinAverageNote { get; set; }
        public string SessionPresident { get; set; }
        public double MinLessonNote { get; set; } 
        public int MinAbsenteeism { get; set; } 


    }
}
