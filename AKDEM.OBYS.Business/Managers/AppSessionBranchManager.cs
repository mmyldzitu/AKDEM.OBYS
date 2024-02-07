using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
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

    public class AppSessionBranchManager : GenericManager<AppSessionBranchCreateDto, AppSessionBranchUpdateDto, AppSessionBranchListDto, AppSessionBranch>, IAppSessionBranchService
    {
        private readonly IMapper _mapper;
        private readonly IUow _uow;
        public AppSessionBranchManager(IMapper mapper,IValidator<AppSessionBranchCreateDto> createDtoValidator, IValidator<AppSessionBranchUpdateDto> updateDtoValidator,IUow uow):base(mapper,createDtoValidator,updateDtoValidator,uow)
        {
            _mapper = mapper;
            _uow = uow;
        }
        public async Task CreateSessionBranchAsync(List<AppSessionBranchCreateDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var entity = _mapper.Map<AppSessionBranch>(dto);
                var result = await _uow.GetRepositry<AppSessionBranch>().FindByFilterAsync(x => x.SessionId == dto.SessionId && x.BranchId == dto.BranchId);
                if (result == null)
                {
                    await _uow.GetRepositry<AppSessionBranch>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                }
                
            }
        }
        public async Task  RemoveSessionBranchesBySessionId(int sessionId)
        {
            var query = _uow.GetRepositry<AppSessionBranch>().GetQuery();
            var entities = await query.Where(x => x.SessionId == sessionId).ToListAsync();
            
            if (entities.Count != 0)
            {
                foreach(var item in entities)
                {
                    _uow.GetRepositry<AppSessionBranch>().Remove(item);
                    
                }
                await _uow.SaveChangesAsync();
            }
        }
    }
}
