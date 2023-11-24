using AKDEM.OBYS.Dto.AppUserDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppUser
{
   public class AppStudentCreateDtoValidator:AbstractValidator<AppStudentCreateDto>
    {
        public AppStudentCreateDtoValidator()
        {

        }
    }
}
