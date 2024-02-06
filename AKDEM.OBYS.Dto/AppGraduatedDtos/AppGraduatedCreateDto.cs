using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppGraduatedDtos
{
    public class AppGraduatedCreateDto : IDto
    {
        public int UserId { get; set; }
        public string President { get; set; }
        public string year { get; set; }
        public string Date { get; set; }
        public string studentName { get; set; }
        public string belgeNo { get; set; }
    }
}
