using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Dto.AppWarningDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class StudentDetailsModel
    {
        public int Id { get; set; }
        public AppStudentListDto AppStudent { get; set; }
        public StudentDetailSessionListModel AppStudentSession { get; set; }
        public int BranchSessionDegree { get; set; }
        public string SessionName { get; set; }

        public int BranchDegree { get; set; }
        public int TotalSessionDegree { get; set; }
        public int TotalDegree { get; set; }
        public List<AppUserSessionLessonUpdateDto> AppUserSessionLessons { get; set; }
        
    }
}
