using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppTeacherUpdateModelValidator:AbstractValidator<AppTeacherUpdateModel>
    {
        public AppTeacherUpdateModelValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.FirstName).MaximumLength(200).NotEmpty().WithMessage("Lütfen İsim Giriniz");
            RuleFor(x => x.SecondName).MaximumLength(200).NotEmpty().WithMessage("Lütfen Soyisim Giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen Mail Adresi Giriniz");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Lütfen Telefon Numarası Giriniz");
        }
    }
}
