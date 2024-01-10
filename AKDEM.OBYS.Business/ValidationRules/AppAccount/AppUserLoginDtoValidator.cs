using AKDEM.OBYS.Dto.AppAccountDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppAccount
{
    public class AppUserLoginDtoValidator : AbstractValidator<AppUserLoginDto>
    {
        public AppUserLoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Kısmı Boş Geçilemez");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Parola Kısmı Boş Geçilemez");

        }
        
    }
}
