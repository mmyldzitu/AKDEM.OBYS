using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppUserSessionLesson
{
    public class AppUserSessionLessonUpdateDtoValidator : AbstractValidator<AppUserSessionLessonUpdateDto>
    {
        public AppUserSessionLessonUpdateDtoValidator()
        {
            RuleFor(x => x.Not)
                .InclusiveBetween(-10, 100)
                .WithMessage("Lütfen 0 ile 100 arasında geçerli bir not değeri giriniz.");


            RuleFor(x => x.Devamsızlık)
                .InclusiveBetween(-10, 1000)
                .WithMessage("Lütfen devamsızlık için uygun bir sayısal değer giriniz.");
                
        }


        

    }
}
