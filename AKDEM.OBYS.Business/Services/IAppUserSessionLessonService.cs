using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.Services
{
    public interface IAppUserSessionLessonService : IGenericService<AppUserSessionLessonCreateDto, AppUserSessionLessonUpdateDto, AppUserSessionLessonListDto, AppUserSessionLesson>
    {
    }
}
