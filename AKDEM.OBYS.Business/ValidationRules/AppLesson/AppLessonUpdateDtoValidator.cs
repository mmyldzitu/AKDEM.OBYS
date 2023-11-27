using AKDEM.OBYS.Dto.AppLessonDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppLesson
{
   public class AppLessonUpdateDtoValidator:AbstractValidator<AppLessonUpdateDto>
    {
        public AppLessonUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Definition).MaximumLength(200).NotEmpty().WithMessage("Lütfen dersin adını 200 karakterden uzun olmayacak şekilde giriniz");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Lütfen dersin hocasını seçiniz.");
        }
    }
}
