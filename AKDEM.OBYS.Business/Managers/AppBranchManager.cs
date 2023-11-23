using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppBranchDtos;
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
    public class AppBranchManager : GenericManager<AppBranchCreateDto, AppBranchUpdateDto, AppBranchListDto, AppBranch>, IAppBranchService
        
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        public AppBranchManager(IMapper mapper, IValidator<AppBranchCreateDto> createDtoValidator, IValidator<AppBranchUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<List<AppBranchListDto>> GetList()
        {
            var query = _uow.GetRepositry<AppBranch>().GetQuery();
            var list = await query.Include(x => x.AppClass).ToListAsync();
            return _mapper.Map<List<AppBranchListDto>>(list);
        }
        
    }
}
