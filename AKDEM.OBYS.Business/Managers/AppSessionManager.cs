using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
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
        private readonly IMapper _mapper;
        public AppSessionManager(IMapper mapper, IValidator<AppSessionCreateDto> createDtoValidator,IValidator<AppSessionUpdateDto> updateDtoValidator, IUow uow):base(mapper,createDtoValidator,updateDtoValidator,uow)
        {
            _uow = uow;
            _mapper = mapper;
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
        public async Task<IResponse<List<AppSessionListDto>>> GetOrderingAsync()
        {
            var data = await _uow.GetRepositry<AppSession>().GetAllAsync( x => x.Id, Common.Enums.OrderByType.DESC);
            var dto = _mapper.Map<List<AppSessionListDto>>(data);
            return new Response<List<AppSessionListDto>>(ResponseType.Success, dto);

        }
    }
}
