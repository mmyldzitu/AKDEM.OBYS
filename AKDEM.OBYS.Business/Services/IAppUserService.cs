using AKDEM.OBYS.Common;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
   public  interface IAppUserService:IGenericService<AppTeacherCreateDto,AppTeacherUpdateDto,AppTeacherListDto,AppUser>
    {
        Task<IResponse<AppTeacherCreateDto>> CreateWithRoleAsync(AppTeacherCreateDto dto, int roleId);
        Task<IResponse<List<AppTeacherListDto>>> GetAllTeacherAsync(int roleId);

    }
}
