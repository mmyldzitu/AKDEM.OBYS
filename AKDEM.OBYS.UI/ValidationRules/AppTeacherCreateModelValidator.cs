using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppTeacherCreateModelValidator:AbstractValidator<AppTeacherCreateModel>
    {
        private readonly IAppUserService _appUserService;
        public AppTeacherCreateModelValidator(IAppUserService appUserService)
        {
            RuleFor(x => x.FirstName).MaximumLength(200).NotEmpty().WithMessage("Lütfen İsim Giriniz");
            RuleFor(x => x.SecondName).MaximumLength(200).NotEmpty().WithMessage("Lütfen Soyisim Giriniz");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen Mail Adresi Giriniz");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Lütfen Telefon Numarası Giriniz");
            RuleFor(x => x)
                   .MustAsync(async (model, token) => await IsEmailAlready(model.Email))
                   .WithMessage("Bu Email Adresi Zaten Kullanımda!");

            
            _appUserService = appUserService;
        }
        private async Task<bool> IsEmailAlready(string definition)
        {
            var control = await _appUserService.IfEmailAlreadyExists(definition);
            return control;
        }
    }
}
