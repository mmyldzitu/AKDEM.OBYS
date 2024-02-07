using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Business.ValidationRules.AppAccount;
using AKDEM.OBYS.Business.ValidationRules.AppUser;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppAccountDtos;
using AKDEM.OBYS.Dto.AppRoleDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Managers
{
   
   public class AppUserManager:GenericManager<AppTeacherCreateDto,AppTeacherUpdateDto,AppTeacherListDto,AppUser>,IAppUserService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AppTeacherCreateDto> _createDtoValidator;
        private readonly IValidator<AppStudentUpdateDto> _createStudentDtoValidator;
        private readonly IValidator<AppUserLoginDto> _loginDtoValidator;
        
        private readonly IUow _uow;

        public AppUserManager(IMapper mapper, IValidator<AppTeacherCreateDto> createDtoValidator, IValidator<AppTeacherUpdateDto> updateDtoValidator, IUow uow, IValidator<AppStudentUpdateDto> createStudentDtoValidator, IValidator<AppUserLoginDto> loginDtoValidator) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _uow = uow;
            _createStudentDtoValidator = createStudentDtoValidator;
            _loginDtoValidator = loginDtoValidator;
        }
        public async Task<bool> IfEmailAlreadyExists(string definition)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Email == definition).SingleOrDefaultAsync();
            if (user == null)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> IsPasswordRight(int userId, string password)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                if (user.Password == password)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task UserPasswordUpdate(int userId, string newPassword)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var user = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (user != null)
            {
                user.Password = newPassword;
            }
            await _uow.SaveChangesAsync();
        }
        public async Task<IResponse<AppTeacherCreateDto>> CreateTeacherWithRoleAsync(AppTeacherCreateDto dto, int roleId)
        {
            var validationResult = _createDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = _mapper.Map<AppUser>(dto);
                await _uow.GetRepositry<AppUser>().CreateAsync(user);
                await _uow.GetRepositry<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppUser = user,
                    RoleId = roleId
                });
                await _uow.SaveChangesAsync();
                return new Response<AppTeacherCreateDto>(ResponseType.Success, dto);

            }
            return new Response<AppTeacherCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<IResponse<List<AppTeacherListDto>>> GetAllTeacherAsync(int roleId, bool status)
        {
            var teacherList = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.AppUserRoles.Any(x => x.RoleId == roleId  ) && x.Status==status);
            if (teacherList == null)
            {
                return new Response<List<AppTeacherListDto>>(ResponseType.NotFound, "Görüntülenecek Öğretmen Bilgisi Bulunamadı");
            }
            var dto = _mapper.Map<List<AppTeacherListDto>>(teacherList);
            return new Response<List<AppTeacherListDto>>(ResponseType.Success, dto);
        }
        public async Task ChangeTeacherStatus(int userId)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var teacher = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (teacher != null)
            {
                if (teacher.Status)
                {
                    teacher.Status = false;
                }
                else
                {
                    teacher.Status = true;
                }

            }
            await _uow.SaveChangesAsync();
        }

        public async Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type, ClassType classType)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            

            if (Enum.GetName(typeof(ClassType),classType) == "Tamamı")
            {
                var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserRoles.Any(x => x.RoleId == (int)type) && x.Status==true && x.ClassId != (int)ClassType.Mezun).ToListAsync();
                if (list.Count != 0)
                {
                    return _mapper.Map<List<AppStudentListDto>>(list);

                }
                else
                {
                    return new List<AppStudentListDto>();

                }

            }
            else if(Enum.GetName(typeof(ClassType), classType) == "Pasif")
            {
                var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.Status==false).ToListAsync();
                if (list.Count != 0)
                {
                    return _mapper.Map<List<AppStudentListDto>>(list);

                }
                else
                {
                    return new List<AppStudentListDto>();

                }
            }
            else
            {
                var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.Status==true && x.AppUserRoles.Any(x => x.RoleId == (int)type) && x.ClassId == (int)classType ).ToListAsync();
                if (list.Count != 0)
                {
                    return _mapper.Map<List<AppStudentListDto>>(list);

                }
                else
                {
                    return new List<AppStudentListDto>();

                }
            }

        }
        public async Task<IResponse<AppStudentUpdateDto>> CreateStudentWithRoleAsync(AppStudentUpdateDto dto, int roleId)
        {
            var validationResult = _createStudentDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = _mapper.Map<AppUser>(dto);
                await _uow.GetRepositry<AppUser>().CreateAsync(user);
                await _uow.GetRepositry<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppUser = user,
                    RoleId = roleId
                });
                await _uow.SaveChangesAsync();
                return new Response<AppStudentUpdateDto>(ResponseType.Success, dto);

            }
            return new Response<AppStudentUpdateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<int> GetUserIdByNameSecondNameandEmail(string name,string secondName,string mail)
        {
            var entity = await _uow.GetRepositry<AppUser>().FindByFilterAsync(x => x.FirstName == name && x.SecondName == secondName && x.Email == mail);
            return entity.Id;
        }
        public async Task<IResponse<AppTeacherListDto>> CheckUserAsync(AppUserLoginDto dto)
        {
            var validationResult = _loginDtoValidator.Validate(dto);
            if (validationResult.IsValid)
            {
                var query = _uow.GetRepositry<AppUser>().GetQuery();
                
                var user = await _uow.GetRepositry<AppUser>().FindByFilterAsync(x => x.Email == dto.Email && x.Password == dto.Password);
                if (user != null)
                {
                    var appUserDto = _mapper.Map<AppTeacherListDto>(user);
                    return new Response<AppTeacherListDto>(ResponseType.Success, appUserDto);
                }
                return new Response<AppTeacherListDto>(ResponseType.NotFound, "Kullanıcı Adı veya Şifre Hatalı");
            }
            return new Response<AppTeacherListDto>(ResponseType.ValidationError, "Kullanıcı Adı veya Şifre Boş Olamaz");
        }
        public async Task<IResponse<List<AppRoleListDto>>> GetRolesByUserIdAsync(int userId)
        {
            var roles = await _uow.GetRepositry<AppRole>().GetAllAsync(x => x.AppUserRoles.Any(x => x.UserId== userId));
            if (roles == null)
            {
                return new Response<List<AppRoleListDto>>(ResponseType.NotFound, "Rol Bulunamadı");
            }
            var dto = _mapper.Map<List<AppRoleListDto>>(roles);
            return new Response<List<AppRoleListDto>>(ResponseType.Success, dto);
        }
        public async Task<string> GetUserNameById(int userId)
        {
            string firstName = "";
            string secondName = "";
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            
            var entity = await query.Where(x => x.Id == userId).SingleOrDefaultAsync();
            if (entity != null)
            {
                if (entity.FirstName != null)
                {
                    firstName = entity.FirstName;
                }
                if (entity.SecondName != null)
                {
                    secondName = entity.SecondName;
                }
            }
            return firstName +" "+ secondName;
        }
        public async Task<List<string>> GetTeacherNameForPresident()
        {
            List<string> teacherDefinitions = new List<string>();
            string definition = "";
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var teachers = await query.Where(x => x.AppUserRoles.Any(x => x.RoleId == 2) && x.Status == true).ToListAsync();
            if (teachers.Count != 0)
            {
                foreach( var teacher in teachers)
                {
                    definition = teacher.DepartReason + " " + teacher.FirstName + " " + teacher.SecondName;
                    teacherDefinitions.Add(definition);
                }
            }
            return teacherDefinitions;
        }

    }
}
