using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
   public class AppUserProfile:Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppTeacherCreateDto, AppUser>().ReverseMap();
            CreateMap<AppTeacherListDto, AppUser>().ReverseMap();
            CreateMap<AppTeacherUpdateDto, AppUser>().ReverseMap();
                
        }
    }
}
