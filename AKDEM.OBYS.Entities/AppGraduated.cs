using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppGraduated:BaseEntity
    {
        public string President { get; set; }
        public string year { get; set; }
        public string Date { get; set; }
        public AppUser AppUser { get; set; }
        public int UserId { get; set; }
        public string studentName { get; set; }
        public string belgeNo { get; set; }
    }
}
