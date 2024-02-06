using AKDEM.OBYS.Business.Services;
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
        private readonly IAppBranchService _appBranchService;
        public AppBranchCreateModelValidator(IAppBranchService appBranchService)
        {
            RuleFor(x => x.Branch).NotEmpty().WithMessage("Lütfen Şube İsmini Giriniz");
            RuleFor(x => x.Class).NotEmpty().WithMessage("Lütfen Sınıf İsmini Giriniz");
            RuleFor(x => x.ClassId).NotEmpty();
            RuleFor(x => x)
       .MustAsync(async (model, token) => await IsBranchAlready(model.Definition))
       .WithMessage("Oluşturmak İstediğiniz İsimle Aktif Bir Sınıf Bulunmakta!");
            _appBranchService = appBranchService;
        }
        private async Task<bool> IsBranchAlready(string definition)
        {
            var control = await _appBranchService.IfBranchAlreadyExists(definition);
            return control;
        }
    }
}
