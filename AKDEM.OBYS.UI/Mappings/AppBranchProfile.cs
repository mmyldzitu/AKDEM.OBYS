using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.UI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.Mappings
{
    public class AppBranchProfile:Profile
    {
        public AppBranchProfile()
        {
            CreateMap<AppBranchCreateModel, AppBranchCreateDto>().ReverseMap();
            CreateMap<AppBranchUpdateModel, AppBranchUpdateDto>().ReverseMap();
        }
    }
}
