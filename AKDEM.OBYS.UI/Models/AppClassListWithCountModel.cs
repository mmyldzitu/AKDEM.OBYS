using AKDEM.OBYS.Dto.AppClassDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppClassListWithCountModel
    {
        public List<AppBranchListWithCountModel> BranchListWithCountModels { get; set; }
        

        public AppClassListDto AppClass { get; set; }
        public int ClassStokes { get; set; }
    }
}
