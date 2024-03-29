﻿using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppWarningDtos
{
    public class AppWarningListDto : IDto
    {
        public int Id { get; set; }
        public double WarningCount { get; set; }
        public DateTime WarningTime { get; set; }
        public string WarningReason { get; set; }
        public AppUserSessionListDto AppUserSession { get; set; }

        public int UserSessionId { get; set; }
        
    }
}
