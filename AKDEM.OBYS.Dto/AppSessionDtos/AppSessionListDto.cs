using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppSessionDtos
{
   public class AppSessionListDto:IDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public bool Status { get; set; }
        public bool Status2 { get; set; }
    }
}
