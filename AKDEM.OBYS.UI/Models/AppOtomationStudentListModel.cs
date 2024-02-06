using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppOtomationStudentListModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public double Average { get; set; }
        public double TotalAverage { get; set; }
        public int BranchSessionDegree { get; set; } = 0;
        public int BranchDegree { get; set; } = 0;
        public int TotalSessionDegree { get; set; } = 0;
        public int TotalDegree { get; set; } = 0;
        public double SessionLessonWarningCount { get; set; } = 0;
        public double SessionAbsentWarningCount { get; set; } = 0;
        public double SessionWarningCount { get; set; } = 0;
        public bool Status { get; set; }
        public double WarningCount { get; set; } = 0;
        public int StudentId { get; set; }
        public int StudentSessionId { get; set; } = 0;
        public int SessionId { get; set; }
    }
}
