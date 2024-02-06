using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Dto.AppLessonDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.ValidationRules.AppLesson
{
    public class AppLessonCreateDtoValidator:AbstractValidator<AppLessonCreateDto>
    {
        private readonly IAppSessionService _appSessionService;
        public AppLessonCreateDtoValidator(IAppSessionService appSessionService)
        {
            RuleFor(x => x.Definition).MaximumLength(200).NotEmpty().WithMessage("Lütfen dersin adını 200 karakterden uzun olmayacak şekilde giriniz");
            RuleFor(x => x.UserId).NotEmpty().WithMessage("Lütfen dersin hocasını seçiniz.");
            RuleFor(x => x)
           .MustAsync(async (model, token) => await IsLessonAlready(model.Definition, model.UserId))
           .WithMessage("Aynı Ders ve Öğretmen İsmiyle Oluşmuş Bir Ders Bulunmakta!");
            
            _appSessionService = appSessionService;
        }
        private async Task<bool> IsLessonAlready(string definition,int userId)
        {
            var control = await _appSessionService.IfLessonAlreadyExists(definition,userId);
            return !control;
        }
    }
}
