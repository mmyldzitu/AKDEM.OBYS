using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserSessionDtos
{
    public class AppUserSessionListDto : IDto
    {
        public int Id { get; set; }
        public double Average { get; set; }
        public int SessionWarningCount { get; set; }
       
        public int UserId { get; set; }
        public AppSessionListDto AppSession { get; set; }
        public int SessionId { get; set; }
        public AppStudentListDto AppUser { get; set; }
        public AppBranchListDto AppBranch { get; set; }

    }
}
