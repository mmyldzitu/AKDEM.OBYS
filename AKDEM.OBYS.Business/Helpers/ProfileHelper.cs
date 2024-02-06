using AKDEM.OBYS.Business.Mappings;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Helpers
{
    public static class ProfileHelper
    {
        public static List<Profile> GetProfiles()
        {
            return new List<Profile> {
            new AppSessionProfile(),
            new AppLessonProfile(),
            new AppUserProfile(),
            new AppBranchProfile(),
            new AppClassProfile(),
            new AppStudentProfile(),
            new AppScheduleProfile(),
            new AppScheduleDetailProfile(),
            new AppUserProfile(),
            new AppUserSessionProfile(),
            new AppUserSessionLessonProfile(),
            new AppWarningProfile(),
            new AppSessionBranchProfile(),
            new AppRoleProfile(),
            new AppGraduatedProfile(),
            
            };

        }
    }
}
