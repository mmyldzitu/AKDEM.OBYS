using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Entities
{
   public class AppUser:BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
        public string ImagePath { get; set; }


        public int? BranchId { get; set; }
        public AppBranch AppBranch { get; set; }

        public int? ClassId { get; set; }
        public AppClass AppClass { get; set; }

        public List<AppUserRole> AppUserRoles { get; set; }
        public List<AppUserSession> AppUserSessions { get; set; }
        public List<AppWarning> AppWarnings { get; set; }
        public List<AppLesson> AppLessons { get; set; }


    }
}
