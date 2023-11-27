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
    public class AppStudentProfile:Profile
    {
        public AppStudentProfile()
        {
            CreateMap<AppStudentUpdateDto, AppUser>().ReverseMap();
            CreateMap<AppStudentCreateDto, AppUser>().ReverseMap();
            CreateMap<AppStudentListDto, AppUser>().ReverseMap();
        }
    }
}
