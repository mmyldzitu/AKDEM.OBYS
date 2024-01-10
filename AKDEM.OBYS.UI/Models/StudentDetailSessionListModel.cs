using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Models
{
    public class StudentDetailSessionListModel
    {
       
        public double Average { get; set; }
        public double TotalAverage { get; set; }
        public double SessionWarningCount { get; set; }
        public double TotalWarningCount { get; set; }
        public AppBranchListDto AppBranch { get; set; }
        public AppClassListDto AppClass { get; set; }
        public int BranchId { get; set; }
        public int ClassId { get; set; }
        public bool Status { get; set; }
        public int UserId { get; set; }

        public int SessionId { get; set; }
    }
}
