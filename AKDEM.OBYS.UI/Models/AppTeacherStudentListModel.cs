using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppTeacherStudentListModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public double Average { get; set; }
        public int SıraNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public double LessonNote { get; set; }
        public int Devamsızlık { get; set; }

        public int LessonBranchDegree { get; set; } = 0;
        
        
        public bool Status { get; set; }
        
        public int StudentId { get; set; }
        public int StudentSessionId { get; set; } = 0;
        public int SessionId { get; set; }
    }
}
