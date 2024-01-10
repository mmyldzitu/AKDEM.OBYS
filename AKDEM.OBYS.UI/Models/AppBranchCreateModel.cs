using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppBranchCreateModel
    {
        public string Definition { get; set; } = "model";
        public string Class { get; set; }
        public string Branch { get; set; }
        public int ClassId { get; set; }
        public bool Status { get; set; } = true;

    }
}
