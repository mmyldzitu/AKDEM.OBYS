using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppClassDtos;
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
        Task<List<AppBranchListDto>> GetClasses(int id);
        Task<List<AppBranchListDto>> GetBranchListNotScheduleAsync(int sessionId);
        Task<int> GetBranchIdByBranchDefinitionAsync(string definition);
        Task<int> GetClassIdByBranchId(int branchId);
        Task<string> BranchNameByByBranchId(int branchId);
        Task<AppClassListDto> GetClassById(int id);
        Task<List<AppBranchListDto>> GetListWithClassId(int classId);
        Task<List<AppBranchListDto>> GetBranchListWithSessionId(int sessionId);
        Task<AppBranchListDto> GetBrancWithId(int branchId);
        Task SessionEndingBranchStatus(int branchId);
        Task<AppBranchListDto> FindBranchByNameAndStatus(string name);
        Task<string> FindClassNameByClassId(int classId);
        Task CreateBranchWhichNotExist(AppBranchCreateDto dto);
    }
    
}
