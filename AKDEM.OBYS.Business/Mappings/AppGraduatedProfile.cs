using AKDEM.OBYS.Dto.AppGraduatedDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppGraduatedProfile:Profile
    {
        public AppGraduatedProfile()
        {
            CreateMap<AppGraduatedListDto, AppGraduated>().ReverseMap();
            CreateMap<AppGraduatedCreateDto, AppGraduated>().ReverseMap();
            CreateMap<AppGraduatedUpdateDto, AppGraduated>().ReverseMap();
        }
    }
}
