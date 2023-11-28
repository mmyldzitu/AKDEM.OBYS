using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
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
    public class AppUserSessionLessonManager : GenericManager<AppUserSessionLessonCreateDto, AppUserSessionLessonUpdateDto, AppUserSessionLessonListDto, AppUserSessionLesson>, IAppUserSessionLessonService
    {
        public AppUserSessionLessonManager(IMapper mapper, IValidator<AppUserSessionLessonCreateDto> createDtoValidator, IValidator<AppUserSessionLessonUpdateDto> updateDtoValidator, IUow uow) : base(mapper, createDtoValidator, updateDtoValidator, uow)
        {

        }
    }
}
