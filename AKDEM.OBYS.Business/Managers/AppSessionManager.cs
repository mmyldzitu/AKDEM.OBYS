using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppSessionDtos;
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
    public class AppSessionManager : GenericManager<AppSessionCreateDto, AppSessionUpdateDto, AppSessionListDto, AppSession>, IAppSessionService
    {
        private readonly IUow _uow;
        public AppSessionManager(IMapper mapper, IValidator<AppSessionCreateDto> createDtoValidator,IValidator<AppSessionUpdateDto> updateDtoValidator, IUow uow):base(mapper,createDtoValidator,updateDtoValidator,uow)
        {
            _uow = uow;
        }
        public async Task SetStatusAsync(int sessionId)
        {
            var query = _uow.GetRepositry<AppSession>().GetQuery();
            var entity = await query.SingleOrDefaultAsync(x => x.Id == sessionId);
            if (entity.Status == true)
            {
                entity.Status = false;

            }
            else
            {
                entity.Status = true;
            }
            await _uow.SaveChangesAsync();
        }
    }
}
