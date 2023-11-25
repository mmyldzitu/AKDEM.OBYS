using AKDEM.OBYS.Dto.AppBranchDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class AppBranchListModel
    {
        public AppBranchListDto AppBranch { get; set; }
        public List<AppBranchListDto> AppBranches { get; set; }
    }
}
