using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionLessonDtos
{
    public class AppUserSessionLessonCreateDto :IDto
    {
        public int LessonId { get; set; }
        
        public int UserSessionId { get; set; }

        public int Not { get; set; } = -1;
        public int Devamsızlık { get; set; } = -1;
    }
}
