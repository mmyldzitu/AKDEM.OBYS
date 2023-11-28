using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppWarningDtos;
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
    public class AppWarningManager : GenericManager<AppWarningCreateDto, AppWarningUpdateDto, AppWarningListDto, AppWarning>, IAppWarningService
    {
        public AppWarningManager(IMapper mapper, IValidator<AppWarningCreateDto> createDtoValidator, IValidator<AppWarningUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {

        }
    }
}
