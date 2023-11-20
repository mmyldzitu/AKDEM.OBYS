using AKDEM.OBYS.Dto.AppSessionDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppSession
{
   public class AppSessionUpdateDtoValidator:AbstractValidator<AppSessionUpdateDto>
    {
        public AppSessionUpdateDtoValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Definition).MaximumLength(200).NotEmpty().WithMessage("Lütfen Dönem İsmini Giriniz");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Lütfen dönem durumunu işaretleyiniz");

        }
    }
}
