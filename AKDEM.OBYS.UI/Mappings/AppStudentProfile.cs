using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Mappings
{
    public class AppStudentProfile:Profile
    {
        public AppStudentProfile()
        {
            CreateMap<AppStudentUpdateModel, AppStudentUpdateDto>().ReverseMap();
            CreateMap<AppStudentCreateModel, AppStudentCreateDto>().ReverseMap();
            CreateMap<AppStudentListModel, AppStudentListDto>().ReverseMap();
        }
    }
}
