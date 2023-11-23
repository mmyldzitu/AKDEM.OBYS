using AKDEM.OBYS.Dto.AppClassDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppClassProfile:Profile
    {
        public AppClassProfile()
        {
            CreateMap<AppClassListDto, AppClass>().ReverseMap();
        }
    }
}
