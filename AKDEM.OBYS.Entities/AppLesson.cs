﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
   public class AppLesson:BaseEntity
    {
        public string Definition { get; set; }
        public int UserId { get; set; }
        public bool Status { get; set; }
        public AppUser AppUser { get; set; }
        public List<AppUserSessionLesson> AppUserSessionLessons { get; set; }
        public List<AppScheduleDetail> AppScheduleDetails { get; set; }
    }
}
