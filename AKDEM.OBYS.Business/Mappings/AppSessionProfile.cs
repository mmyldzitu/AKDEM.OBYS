using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppSessionProfile:Profile
    {
        public AppSessionProfile()
        {
            CreateMap<AppSessionCreateDto, AppSession>().ReverseMap();
            CreateMap<AppSessionListDto, AppSession>().ReverseMap();
            CreateMap<AppSessionUpdateDto, AppSession>().ReverseMap();
        }
    }
}
