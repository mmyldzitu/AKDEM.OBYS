using AKDEM.OBYS.Business.Managers;
using AKDEM.OBYS.Business.Mappings;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Business.ValidationRules.AppAccount;
using AKDEM.OBYS.Business.ValidationRules.AppBranch;
using AKDEM.OBYS.Business.ValidationRules.AppLesson;
using AKDEM.OBYS.Business.ValidationRules.AppSchedule;
using AKDEM.OBYS.Business.ValidationRules.AppScheduleDetail;
using AKDEM.OBYS.Business.ValidationRules.AppSession;
using AKDEM.OBYS.Business.ValidationRules.AppSessionBranch;
using AKDEM.OBYS.Business.ValidationRules.AppUser;
using AKDEM.OBYS.Business.ValidationRules.AppUserSession;
using AKDEM.OBYS.Business.ValidationRules.AppUserSessionLesson;
using AKDEM.OBYS.Business.ValidationRules.AppWarning;
using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppAccountDtos;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppScheduleDetailDto;
using AKDEM.OBYS.Dto.AppScheduleDtos;
using AKDEM.OBYS.Dto.AppSessionBranchDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AKDEM.OBYS.Dto.AppUserSessionDtos;
using AKDEM.OBYS.Dto.AppUserSessionLessonDtos;
using AKDEM.OBYS.Dto.AppWarningDtos;
using AutoMapper;
using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.Business.DependencyResolvers
{
    public static class DependencyExtension
    {
        public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AkdemContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Local"));

                //mapping profilleri buraya

            }
                );




            services.AddScoped<IUow, Uow>();
            services.AddScoped<IAppSessionService, AppSessionManager>();
            services.AddScoped<IAppUserService, AppUserManager>();
            services.AddScoped<IAppStudentService, AppStudentManager>();
            services.AddScoped<IAppBranchService, AppBranchManager>();
            services.AddScoped<IAppLessonService, AppLessonManager>();
            services.AddScoped<IAppScheduleService, AppScheduleManager>();
            services.AddScoped<IAppScheduleDetailService, AppScheduleDetailManager>();
            services.AddScoped<IAppUserSessionLessonService, AppUserSessionLessonManager>();
            services.AddScoped<IAppUserSessionService, AppUserSessionManager>();
            services.AddScoped<IAppWarningService, AppWarningManager>();
            services.AddScoped<IAppSessionBranchService, AppSessionBranchManager>();


            services.AddTransient<IValidator<AppLessonCreateDto>, AppLessonCreateDtoValidator>();
            services.AddTransient<IValidator<AppLessonUpdateDto>, AppLessonUpdateDtoValidator>();
            services.AddTransient<IValidator<AppSessionCreateDto>, AppSessionCreateDtoValidator>();
            services.AddTransient<IValidator<AppSessionUpdateDto>, AppSessionUpdateDtoValidator>();

            services.AddTransient<IValidator<AppTeacherCreateDto>, AppTeacherCreateDtoValidator>();
            services.AddTransient<IValidator<AppTeacherUpdateDto>, AppTeacherUpdateDtoValidator>();
            services.AddTransient<IValidator<AppBranchCreateDto>, AppBranchCreateDtoValidator>();
            services.AddTransient<IValidator<AppBranchUpdateDto>, AppBranchUpdateDtoValidator>();
            services.AddTransient<IValidator<AppStudentUpdateDto>, AppStudentUpdateDtoValidator>();
            services.AddTransient<IValidator<AppStudentCreateDto>, AppStudentCreateDtoValidator>();

            services.AddTransient<IValidator<AppScheduleCreateDto>, AppScheduleCreateDtoValidator>();
            services.AddTransient<IValidator<AppScheduleUpdateDto>, AppScheduleUpdateDtoValidator>();

            services.AddTransient<IValidator<AppScheduleDetailUpdateDto>, AppScheduleDetailUpdateDtoValidator>();
            services.AddTransient<IValidator<AppScheduleDetailCreateDto>, AppScheduleDetailCreateDtoValidator>();

            services.AddTransient<IValidator<AppUserSessionCreateDto>, AppUserSessionCreateDtoValidator>();
            services.AddTransient<IValidator<AppUserSessionUpdateDto>, AppUserSessionUpdateDtoValidator>();

            services.AddTransient<IValidator<AppUserSessionLessonUpdateDto>, AppUserSessionLessonUpdateDtoValidator>();
            services.AddTransient<IValidator<AppUserSessionLessonCreateDto>, AppUserSessionLessonCreateDtoValidator>();

            services.AddTransient<IValidator<AppWarningCreateDto>, AppWarningCreateDtoValidator>();
            services.AddTransient<IValidator<AppWarningUpdateDto>, AppWarningUpdateDtoValidator>();

            services.AddTransient<IValidator<AppSessionBranchUpdateDto>, AppSessionBranchUpdateDtoValidator>();
            services.AddTransient<IValidator<AppSessionBranchCreateDto>, AppSessionBranchCreateDtoValidator>();

            services.AddTransient<IValidator<AppUserLoginDto>, AppUserLoginDtoValidator>();

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));





            //dependecy injectionlar ve validaionlar buraya
        }

    }
}
