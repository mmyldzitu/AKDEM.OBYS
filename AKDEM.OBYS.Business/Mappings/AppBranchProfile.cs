using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
   public class AppBranchProfile:Profile
    {
        public AppBranchProfile()
        {
            CreateMap<AppBranchListDto, AppBranch>().ReverseMap();
            CreateMap<AppBranchCreateDto, AppBranch>().ReverseMap();
            CreateMap<AppBranchUpdateDto, AppBranch>().ReverseMap();
        }
    }
}
