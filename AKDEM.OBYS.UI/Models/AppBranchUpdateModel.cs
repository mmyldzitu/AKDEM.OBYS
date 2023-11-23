using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppBranchUpdateModel
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int ClassId { get; set; }
        public SelectList Classes { get; set; }
    }
}
