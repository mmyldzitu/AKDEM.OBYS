using AKDEM.OBYS.Dto.AppWarningDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppWarning
{
    public class AppWarningCreateDtoValidator : AbstractValidator<AppWarningCreateDto>
    {
        public AppWarningCreateDtoValidator()
        {

        }
    }
}
