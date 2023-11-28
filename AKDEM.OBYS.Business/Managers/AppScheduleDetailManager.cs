using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
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
    public class AppScheduleDetailManager : GenericManager<AppScheduleDetailCreateDto, AppScheduleDetailUpdateDto, AppScheduleDetailListDto, AppScheduleDetail>, IAppScheduleDetailService
    {
        public AppScheduleDetailManager(IMapper mapper, IValidator<AppScheduleDetailCreateDto> createDtoValidator, IValidator<AppScheduleDetailUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {

        }
    }
}
