using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppLessonDtos
{
    public class AppLessonUpdateDto:IUpdateDto
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public int UserId { get; set; }
    }
}
