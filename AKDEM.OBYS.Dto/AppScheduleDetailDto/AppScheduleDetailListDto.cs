using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppScheduleDetailDto
{
    public class AppScheduleDetailListDto : IDto
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public string Hours { get; set; }
        public int LessonId { get; set; }
        public AppLessonListDto AppLesson { get; set; }

        public AppScheduleListDto AppSchedule { get; set; }
    }
}
