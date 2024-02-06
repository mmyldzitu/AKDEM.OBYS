using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class UpdatePasswordModelValidator:AbstractValidator<UpdatePasswordModel>
    {
        private readonly IAppUserService _appUserService;
        public UpdatePasswordModelValidator(IAppUserService appUserService)
        {
            RuleFor(x => x.ExPassword).NotEmpty().WithMessage("Mevcut Şifre Kısmı Boş Geçilemez");
            RuleFor(x => x.NewPassword).NotEmpty().WithMessage("Yeni Şifre Kısmı Boş Geçilemez");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Şifre Tekrarı Kısmı Boş Geçilemez");
            RuleFor(x => x)
       .MustAsync(async (model, token) => await IsPasswordRight(model.UserId, model.ExPassword))
       .WithMessage("Mevcut Şifrenizi Yanlış Girdiniz!");

            RuleFor(x =>
            new { x.NewPassword, x.ConfirmPassword }).Must(x => IsConfirm(x.NewPassword, x.ConfirmPassword)).WithMessage("Şifreler Eşleşmemektedir!").When(x => x.NewPassword != null && x.ConfirmPassword != null);

            _appUserService = appUserService;
        }
        private async Task<bool> IsPasswordRight(int userId, string exPassword)
        {
            var control = await _appUserService.IsPasswordRight(userId, exPassword);
            return control;
        }
        private bool IsConfirm(string newPassword, string confirmPassword)
        {
            if (newPassword == confirmPassword) { return true; }
            return false;
        }
    }
}
