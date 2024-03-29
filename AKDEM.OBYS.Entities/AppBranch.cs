﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
    public class AppBranch:BaseEntity
    {
        public string Definition { get; set; }
        public int ClassId { get; set; }
        public AppClass AppClass { get; set; }
        public List<AppUser> AppUsers { get; set; }
        public List<AppSessionBranch> AppSessionBranches { get; set; }
        public List<AppUserSession> AppUserSessions { get; set; }
        public bool Status { get; set; }

    }
}
