using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppScheduleDetailProfile : Profile
    {
        public AppScheduleDetailProfile()
        {
            CreateMap<AppScheduleDetailListDto, AppScheduleDetail>().ReverseMap();
            CreateMap<AppScheduleDetailUpdateDto, AppScheduleDetail>().ReverseMap();
            CreateMap<AppScheduleDetailCreateDto, AppScheduleDetail>().ReverseMap();

            CreateMap<AppScheduleDetailListDtoDeveloper, AppScheduleDetail>().ReverseMap();
        }
    }
}
