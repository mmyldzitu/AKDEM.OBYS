using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppScheduleProfile:Profile
    {
        public AppScheduleProfile()
        {
            CreateMap<AppScheduleListDto, AppSchedule>().ReverseMap();
            CreateMap<AppScheduleCreateDto, AppSchedule>().ReverseMap();
            CreateMap<AppScheduleUpdateDto, AppSchedule>().ReverseMap();

            CreateMap<AppScheduleListDtoDeveloper, AppSchedule>().ReverseMap();
        }
    }
}
