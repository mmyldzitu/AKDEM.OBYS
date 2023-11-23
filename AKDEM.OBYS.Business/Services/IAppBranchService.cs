using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
   public interface IAppBranchService: IGenericService<AppBranchCreateDto, AppBranchUpdateDto, AppBranchListDto, AppBranch>
    {
        Task<List<AppBranchListDto>> GetList();
    }
    
}
