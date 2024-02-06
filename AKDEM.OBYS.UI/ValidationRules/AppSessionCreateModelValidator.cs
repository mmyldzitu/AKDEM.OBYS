using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Common.Enums;
using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class AppSessionCreateModelValidator: AbstractValidator<AppSessionCreateModel>
    {
        private readonly IAppSessionService _appSessionService;

       

        public AppSessionCreateModelValidator(IAppSessionService appSessionService)
        {
            _appSessionService = appSessionService;

            RuleFor(x => x.year1).NotEmpty().WithMessage("Lütfen Yılı Giriniz");
            RuleFor(x => x.SessionPresident)
    .Must(SessionPresident => SessionPresident != "0")
    .WithMessage("Lütfen Aktif AKDEM Başkanını Seçiniz");
            
            RuleFor(x => x.year2).NotEmpty().WithMessage("Lütfen Yılı Giriniz");
            RuleFor(x => x.SessionType).NotEmpty().WithMessage("Lütfen Dönem Türünü Giriniz");
            RuleFor(x => x)
       .MustAsync(async (model, token) => await IsUnique(model.year1, model.year2, model.SessionType))
       .WithMessage("Oluşturmak İstediğiniz Dönem Zaten Var!");
        }

        private async Task<bool> IsUnique(string year1, string year2, int sessionType)
        {
            string definition = $"{year1}-{year2}/{Enum.GetName(typeof(SessionType), sessionType)}";
            var control = await _appSessionService.IsThereSession(definition);
            return control;
        }
    }
}
