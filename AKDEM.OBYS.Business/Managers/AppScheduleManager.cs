using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Entities;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Managers
{
   public class AppScheduleManager:GenericManager<AppScheduleCreateDto,AppScheduleUpdateDto,AppScheduleListDto,AppSchedule>,IAppScheduleService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        
        public AppScheduleManager(IMapper mapper, IValidator<AppScheduleCreateDto> createDtoValidator, IValidator<AppScheduleUpdateDto> updateDtoValidator, IUow uow):base(mapper,createDtoValidator,updateDtoValidator,uow)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IResponse<List<AppScheduleListDto>>> GetSchedulesBySessionId(int id)
        {
            var data = await _uow.GetRepositry<AppSchedule>().GetAllAsync(x => x.SessionId == id);
            if (data == null)
            {
                return new Response<List<AppScheduleListDto>>(ResponseType.NotFound, "Döneme Ait Ders Programı Bulunamadı");
            }

            var dto = _mapper.Map<List<AppScheduleListDto>>(data);
            return new Response<List<AppScheduleListDto>>(ResponseType.Success, dto);
        }
    }
}
