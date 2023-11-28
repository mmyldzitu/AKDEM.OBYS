using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppUserSessionProfile : Profile
    {
        public AppUserSessionProfile()
        {
            CreateMap<AppUserSessionListDto, AppUserSession>().ReverseMap();
            CreateMap<AppUserSessionCreateDto, AppUserSession>().ReverseMap();
            CreateMap<AppUserSessionUpdateDto, AppUserSession>().ReverseMap();
        }
    }
}
