using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppUserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppUser
{
    public class AppStudentCreateDtoValidator : AbstractValidator<AppStudentCreateDto>
    {

        public AppStudentCreateDtoValidator(IAppUserService appUserService)
        {
            RuleFor(x => x.FirstName).MaximumLength(200).NotEmpty().WithMessage("Lütfen İsim Giriniz");
            RuleFor(x => x.SecondName).MaximumLength(200).NotEmpty().WithMessage("Lütfen Soyisim Giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen Mail Adresi Giriniz");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Lütfen Telefon Numarası Giriniz");
            RuleFor(x => x.ClassId).NotEmpty().WithMessage("Lütfen Sınıf Seçiminizi Yapınız");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Lütfen Şube Seçiminizi Yapınız");


        }
    }
}