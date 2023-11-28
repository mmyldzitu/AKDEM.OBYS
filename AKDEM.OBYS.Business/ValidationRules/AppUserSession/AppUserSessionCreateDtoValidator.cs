using AKDEM.OBYS.Dto.AppUserSessionDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppUserSession
{
    public class AppUserSessionCreateDtoValidator : AbstractValidator<AppUserSessionCreateDto>
    {
        public AppUserSessionCreateDtoValidator()
        {

        }
    }
}
