using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Mappings
{
    public class AppTeacherProfile:Profile
    {
        public AppTeacherProfile()
        {
            CreateMap<AppTeacherCreateModel, AppTeacherCreateDto>().ReverseMap();
            CreateMap<AppTeacherUpdateModel, AppTeacherUpdateDto>().ReverseMap();
        }
    }
}
