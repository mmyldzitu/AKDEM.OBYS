using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppSessionDtos;
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
    public class AppSessionManager : GenericManager<AppSessionCreateDto, AppSessionUpdateDto, AppSessionListDto, AppSession>, IAppSessionService
    {
        public AppSessionManager(IMapper mapper, IValidator<AppSessionCreateDto> createDtoValidator,IValidator<AppSessionUpdateDto> updateDtoValidator, IUow uow):base(mapper,createDtoValidator,updateDtoValidator,uow)
        {

        }
    }
}
