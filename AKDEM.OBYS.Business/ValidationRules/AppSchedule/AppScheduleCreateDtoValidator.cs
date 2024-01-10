using AKDEM.OBYS.Dto.AppScheduleDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppSchedule
{
    public class AppScheduleCreateDtoValidator:AbstractValidator<AppScheduleCreateDto>
    {
        public AppScheduleCreateDtoValidator()
        {
            RuleFor(x => x.Definition).NotEmpty().WithMessage("Ders İsmi Boş Geçilemez");
            RuleFor(x => x.SessionBranchId).NotEmpty().WithMessage("Lütfen Bir Sınıf Seçiniz");

        }
    }
}
