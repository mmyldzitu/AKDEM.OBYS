using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppBranchCreateModelValidator:AbstractValidator<AppBranchCreateModel>
    {
        public AppBranchCreateModelValidator()
        {
            RuleFor(x => x.Definition).NotEmpty().WithMessage("Lütfen Şube İsmini Giriniz");
            RuleFor(x => x.ClassId).NotEmpty().WithMessage("Lütfen Şube İiçin Bir Sınıf Seçiniz");
        }
    }
}
