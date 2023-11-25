using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.SessionType
{
   public class SessionTypeListDto:IDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
    }
}
