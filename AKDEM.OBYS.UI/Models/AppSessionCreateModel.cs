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

        public int SessionType { get; set; }
        public bool Status { get; set; } = false;
    }
}
