using AKDEM.OBYS.UI.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI.ValidationRules
{
    public class MergeBranchModelValidator:AbstractValidator<MergeBranchModel>
    {
        public MergeBranchModelValidator()
        {
            RuleFor(x => x.FirstBranchId).NotEmpty().WithMessage("Lütfen Taşımak İstediğiniz Sınıfı Seçiniz");
            RuleFor(x => x.SecondBranchId).NotEmpty().WithMessage("Lütfen Hanigi Sınıfa Taşımak İstediğinizi  Seçiniz").NotEqual(x => x.FirstBranchId).WithMessage("Lütfen Farklı Sınıflar Seçin");

        }
    }
}
