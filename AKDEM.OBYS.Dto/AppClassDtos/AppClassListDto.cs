using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppClassDtos
{
   public class AppClassListDto:IDto
    {
        public int ClassId { get; set; }
        public string Definition { get; set; }
    }
}
