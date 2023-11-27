using AKDEM.OBYS.Dto.AppUserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppLessonListModel
    {
        public int Id { get; set; }
       
        public string Definition { get; set; }
        public int UserId { get; set; }
        public AppTeacherListDto AppTeachers { get; set; }
        public string TeacherName { get; set; }
    }
}
