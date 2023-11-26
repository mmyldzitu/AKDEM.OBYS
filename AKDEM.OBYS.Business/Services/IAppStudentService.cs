using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppStudentService : IGenericService<AppStudentCreateDto, AppStudentUpdateDto, AppStudentListDto, AppUser>
    {
        
        Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type);

        Task<IResponse<AppStudentUpdateDto>> CreateStudentWithRoleAsync(AppStudentUpdateDto dto, int roleId);
        Task<List<AppStudentListDto>> GetStudentsWithBranchAsync(int id);



    }
}
