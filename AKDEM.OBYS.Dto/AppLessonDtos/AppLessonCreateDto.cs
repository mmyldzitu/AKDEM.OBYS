using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AKDEM.OBYS.Dto.AppLessonDtos
{
   public class AppLessonCreateDto:IDto
    {
        public string Definition { get; set; }
        public int UserId { get; set; }
        public SelectList Teachers { get; set; }
    }
}
