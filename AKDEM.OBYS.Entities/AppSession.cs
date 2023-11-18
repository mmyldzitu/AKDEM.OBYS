using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppSession:BaseEntity
    {
        public string Definition { get; set; }
        public List<AppUserSession> AppUserSessions { get; set; }
    }
}
