using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Business.ValidationRules.AppUser;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.UnitOfWork;
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
        private readonly IValidator<AppStudentCreateDto> _createStudentDtoValidator;
        private readonly IUow _uow;

        public AppUserManager(IMapper mapper, IValidator<AppTeacherCreateDto> createDtoValidator, IValidator<AppTeacherUpdateDto> updateDtoValidator, IUow uow, IValidator<AppStudentCreateDto> createStudentDtoValidator) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _uow = uow;
            _createStudentDtoValidator = createStudentDtoValidator;
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
        public async Task<IResponse<List<AppTeacherListDto>>> GetAllTeacherAsync(int roleId)
        {
            var teacherList = await _uow.GetRepositry<AppUser>().GetAllAsync(x => x.AppUserRoles.Any(x => x.RoleId == roleId));
            if (teacherList == null)
            {
                return new Response<List<AppTeacherListDto>>(ResponseType.NotFound, "Görüntülenecek Öğretmen Bilgisi Bulunamadı");
            }
            var dto = _mapper.Map<List<AppTeacherListDto>>(teacherList);
            return new Response<List<AppTeacherListDto>>(ResponseType.Success, dto);
        }

        public async Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserRoles.Any(x => x.RoleId == (int)type)).ToListAsync();
            return _mapper.Map<List<AppStudentListDto>>(list);
        }
        public async Task<IResponse<AppStudentCreateDto>> CreateStudentWithRoleAsync(AppStudentCreateDto dto, int roleId)
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
                return new Response<AppStudentCreateDto>(ResponseType.Success, dto);

            }
            return new Response<AppStudentCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }


    }
}
