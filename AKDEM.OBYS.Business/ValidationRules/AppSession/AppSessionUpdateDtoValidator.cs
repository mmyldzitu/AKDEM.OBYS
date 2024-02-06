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
            RuleFor(x => x.SessionPresident)
    .Must(SessionPresident => SessionPresident != "0")
    .WithMessage("Lütfen Aktif AKDEM Başkanını Seçiniz");
            RuleFor(x => x.Definition).MaximumLength(200).NotEmpty().WithMessage("Lütfen Dönem İsmini Giriniz");
            RuleFor(x => x.MinAbsenteeism).NotEmpty().WithMessage("Lütfen Max Devamsızlık Hakkını Giriniz");
            RuleFor(x => x.MinAverageNote).NotEmpty().WithMessage("Lütfen Ortalama Başarı Kriterini Giriniz");
            RuleFor(x => x.MinLessonNote).NotEmpty().WithMessage("Lütfen Ders Başarı Kriterini Giriniz");
            

        }
    }
}
