using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Dto.AppUserDtos
{
    public class AppUserUpdateDtoDeveloper
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }
        public int ClassId { get; set; }
        public int SıraNo { get; set; }
        public double TotalAverage { get; set; }
        public string DepartReason { get; set; }

        public int BranchId { get; set; }
        public double TotalWarningCount { get; set; }
    }
}
