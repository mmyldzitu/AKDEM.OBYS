﻿using AKDEM.OBYS.Dto.AppGraduatedDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppGraduated
{
    public class AppGraduatedUpdateDtoValidator:AbstractValidator<AppGraduatedUpdateDto>
    {
        public AppGraduatedUpdateDtoValidator()
        {

        }
    }
}
