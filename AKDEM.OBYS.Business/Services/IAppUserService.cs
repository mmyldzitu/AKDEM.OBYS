using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.Dto.AppAccountDtos;
using AKDEM.OBYS.Dto.AppRoleDtos;
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
        Task<IResponse<AppTeacherCreateDto>> CreateTeacherWithRoleAsync(AppTeacherCreateDto dto, int roleId);
        Task<IResponse<List<AppTeacherListDto>>> GetAllTeacherAsync(int roleId,bool status);

        Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type, ClassType classType);
        Task<IResponse<AppStudentUpdateDto>> CreateStudentWithRoleAsync(AppStudentUpdateDto dto, int roleId);
        Task<int> GetUserIdByNameSecondNameandEmail(string name, string secondName, string mail);
        Task<IResponse<AppTeacherListDto>> CheckUserAsync(AppUserLoginDto dto);
        Task<IResponse<List<AppRoleListDto>>> GetRolesByUserIdAsync(int userId);
        Task<string> GetUserNameById(int userId);
        Task<bool> IsPasswordRight(int userId, string password);
        Task UserPasswordUpdate(int userId, string newPassword);
        Task ChangeTeacherStatus(int userId);
        Task<List<string>> GetTeacherNameForPresident();
        Task<bool> IfEmailAlreadyExists(string definition);
        Task<List<AppUserListDtoDeveloper>> GetAppUsersDeveloper();
        Task<AppUserListDtoDeveloper> GetAppUserDeveloper(int id);
        Task UpdateUserDeveloper(AppUserUpdateDtoDeveloper dto);
        Task<string> ReturnImagePathofUser(int userId);
        Task UpdatePPUser(string path, int userId);





    }
}
