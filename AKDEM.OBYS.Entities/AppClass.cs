using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
   public class AppClass:BaseEntity
    {
        public string Definition { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<AppBranch> AppBranches { get; set; }
    }
}
