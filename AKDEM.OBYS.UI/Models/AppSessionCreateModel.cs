using AKDEM.OBYS.Common.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppSessionCreateModel
    {
        public string Definition { get; set; }
        public string year1 { get; set; }
        public string year2 { get; set; }
        public string SessionPresident { get; set; }
        public int SessionType { get; set; }
        public bool Status { get; set; } = true;
        public bool Status2 { get; set; } = true;
        public double MinAverageNote { get; set; } = 70;
        public double MinLessonNote { get; set; } = 50;
        public int MinAbsenteeism { get; set; } = 3;
    }
}
