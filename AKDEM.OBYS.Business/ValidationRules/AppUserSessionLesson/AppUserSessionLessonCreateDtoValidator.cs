using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppUserSessionLesson
{
    public class AppUserSessionLessonCreateDtoValidator : AbstractValidator<AppUserSessionLessonCreateDto>
    {
        public AppUserSessionLessonCreateDtoValidator()
        {

        }
    }
}
