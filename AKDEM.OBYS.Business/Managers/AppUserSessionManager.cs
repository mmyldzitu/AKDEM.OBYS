using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
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
    public class AppUserSessionManager : GenericManager<AppUserSessionCreateDto, AppUserSessionUpdateDto, AppUserSessionListDto, AppUserSession>, IAppUserSessionService
    {
        public AppUserSessionManager(IMapper mapper, IValidator<AppUserSessionCreateDto> createDtoValidator, IValidator<AppUserSessionUpdateDto> updateDtovalidator, IUow uow) : base(mapper, createDtoValidator, updateDtovalidator, uow)
        {

        }
        
    }
}
