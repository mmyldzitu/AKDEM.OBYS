using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppScheduleDetailsTeacherModel
    {
        public int ıd { get; set; }
        public string Day { get; set; }
        public string Hours { get; set; }
        public int LessonId { get; set; }
        public AppLessonListDto AppLesson { get; set; }
        public string BranchDefinition { get; set; }
        public AppScheduleListDto AppSchedule { get; set; }
    }
}
