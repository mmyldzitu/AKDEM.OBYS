using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppScheduleDtos
{
    public class AppScheduleListDtoDeveloper
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int SessionBranchId { get; set; }
    }
}
