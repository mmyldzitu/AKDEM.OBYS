﻿using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserDtos
{
   public  class AppStudentListDto:IDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }
        public int ClassId { get; set; }
        public int SıraNo { get; set; }
        public double TotalAverage { get; set; }
        public string DepartReason { get; set; }
        public AppClassListDto AppClass { get; set; }
        public int BranchId { get; set; }
        public double TotalWarningCount { get; set; }
        public List<AppUserSessionListDto> AppUserSessions { get; set; }
        public AppBranchListDto AppBranch { get; set; }
    }
}
