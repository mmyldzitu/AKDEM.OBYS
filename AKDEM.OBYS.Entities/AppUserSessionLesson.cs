using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
   public class AppUserSessionLesson:BaseEntity
    {
        public int LessonId { get; set; }
        public AppLesson AppLesson { get; set; }
        public int UserSessionId { get; set; }
        public AppUserSession AppUserSession { get; set; }
        public int Not { get; set; }
        public int Devamsızlık { get; set; }
        
    }
}
