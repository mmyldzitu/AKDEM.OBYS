using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppSessionBranch : BaseEntity
    {
        public int SessionId { get; set; }
        public AppSession AppSession { get; set; }
        public int BranchId { get; set; }
        public AppBranch AppBranch { get; set; }
        public List<AppSchedule> AppSchedules { get; set; }
    }
}
