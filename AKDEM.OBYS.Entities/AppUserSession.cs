using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppUserSession:BaseEntity
    {
        public double Average { get; set; }
        public int SessionWarningCount { get; set; }
        public List<AppUserSessionLesson> AppUserSessionLessons { get; set; }
        public int UserId { get; set; }
        public AppUser AppUser { get; set; }
        public int SessionId { get; set; }
        public AppSession AppSession { get; set; }
    }
}
