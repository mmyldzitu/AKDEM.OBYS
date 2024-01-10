using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Mappings
{
    public class AppSessionBranchProfile : Profile
    {
        public AppSessionBranchProfile()
        {
            CreateMap<AppSessionBranchListDto, AppSessionBranch>().ReverseMap();
            CreateMap<AppSessionBranchUpdateDto, AppSessionBranch>().ReverseMap();
            CreateMap<AppSessionBranchCreateDto, AppSessionBranch>().ReverseMap();
        }
    }
}
