using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppScheduleDetailCreateModelValidator : AbstractValidator<AppScheduleDetailCreateModel>
    {
        public AppScheduleDetailCreateModelValidator()
        {

            RuleFor(x => x.LessonId).NotEmpty().WithMessage("Lütfen Bir Ders Seçiniz");
            RuleFor(x => x.Day)
    .Must(day => day != "0")
    .WithMessage("Lütfen Bir Gün Seçiniz");
            RuleFor(x => x.FirstHour).NotEmpty().WithMessage("Lütfen Bir Başlangıç Saati Seçiniz");
            RuleFor(x => x.LastHour).NotEmpty().WithMessage("Lütfen Bir Bitiş Saati Seçiniz");
        }
    }
}
