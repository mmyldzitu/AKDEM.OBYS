using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionLessonDtos
{
    public class AppUserSessionLessonUpdateDto : IUpdateDto
    {
        public int Id { get ; set; }
        public int LessonId { get; set; }
        public AppLessonListDto AppLesson { get; set; }
        public int UserSessionId { get; set; }

        public int Not { get; set; }
        public int Devamsızlık { get; set; }
    }
}
