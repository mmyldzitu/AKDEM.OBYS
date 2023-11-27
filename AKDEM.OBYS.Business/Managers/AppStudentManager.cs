using AKDEM.OBYS.Business.Extensions;
using AKDEM.OBYS.Business.Services;
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
    public class AppStudentManager : GenericManager<AppStudentCreateDto, AppStudentUpdateDto, AppStudentListDto, AppUser>, IAppStudentService
    {
        private readonly IMapper _mapper;
        private readonly IValidator<AppStudentCreateDto> _createDtoValidator;
        private readonly IValidator<AppStudentUpdateDto> _updateDtoValidator;
        private readonly IUow _uow;

        public AppStudentManager(IMapper mapper, IValidator<AppStudentCreateDto> createDtoValidator, IValidator<AppStudentUpdateDto> updateDtoValidator, IUow uow, IValidator<AppStudentUpdateDto> createStudentDtoValidator) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _mapper = mapper;
            _createDtoValidator = createDtoValidator;
            _uow = uow;
            _updateDtoValidator = createStudentDtoValidator;
        }
        
       

        public async Task<List<AppStudentListDto>> GetAllStudentAsync(RoleType type)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.AppUserRoles.Any(x => x.RoleId == (int)type)).ToListAsync();
            return _mapper.Map<List<AppStudentListDto>>(list);
        }
        public async Task<IResponse<AppStudentCreateDto>> CreateStudentWithRoleAsync(AppStudentCreateDto dto, int roleId)
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
                return new Response<AppStudentCreateDto>(ResponseType.Success, dto);

            }
            return new Response<AppStudentCreateDto>(dto, validationResult.ConvertToCustomValidationError());
        }
        public async Task<List<AppStudentListDto>> GetStudentsWithBranchAsync(int id)
        {
            var query = _uow.GetRepositry<AppUser>().GetQuery();
            var list = await query.Include(x => x.AppBranch).ThenInclude(x => x.AppClass).Where(x => x.BranchId==id).ToListAsync();
            return _mapper.Map<List<AppStudentListDto>>(list);
        }


    }
}
