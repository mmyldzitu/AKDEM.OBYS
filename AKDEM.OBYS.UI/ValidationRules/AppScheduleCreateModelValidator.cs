using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppScheduleCreateModelValidator:AbstractValidator<AppScheduleCreateModel>
    {
        public AppScheduleCreateModelValidator()
        {
            RuleFor(x => x.Definition).NotEmpty().WithMessage("Lütfen Program İsmini Giriniz");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Lütfen Program İçin Bir Sınıf Seçiniz");
        }
    }
}
