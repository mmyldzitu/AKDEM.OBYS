using AKDEM.OBYS.Dto.AppWarningDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppWarningProfile : Profile
    {
        public AppWarningProfile()
        {
            CreateMap<AppWarningListDto, AppWarning>().ReverseMap();
            CreateMap<AppWarningCreateDto, AppWarning>().ReverseMap();
            CreateMap<AppWarningUpdateDto, AppWarning>().ReverseMap();
        }
    }
}
