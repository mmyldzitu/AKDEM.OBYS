using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppBranchListWithCountModel
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int ClassId { get; set; }

        public AppClassListDto AppClass { get; set; }
        public int BranchStokes { get; set; }
    }
}
