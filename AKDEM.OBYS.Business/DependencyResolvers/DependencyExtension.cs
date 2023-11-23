using AKDEM.OBYS.Business.Managers;
using AKDEM.OBYS.Business.Mappings;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Business.ValidationRules.AppBranch;
using AKDEM.OBYS.Business.ValidationRules.AppLesson;
using AKDEM.OBYS.Business.ValidationRules.AppSession;
using AKDEM.OBYS.Business.ValidationRules.AppUser;
using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppBranchDtos;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
using AKDEM.OBYS.Dto.AppUserDtos;
using AutoMapper;
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
            services.AddScoped<IAppBranchService, AppBranchManager>();

            services.AddTransient<IValidator<AppLessonCreateDto>, AppLessonCreateDtoValidator>();
            services.AddTransient<IValidator<AppSessionCreateDto>, AppSessionCreateDtoValidator>();
            services.AddTransient<IValidator<AppSessionUpdateDto>, AppSessionUpdateDtoValidator>();

            services.AddTransient<IValidator<AppTeacherCreateDto>, AppTeacherCreateDtoValidator>();
            services.AddTransient<IValidator<AppTeacherUpdateDto>, AppTeacherUpdateDtoValidator>();
            services.AddTransient<IValidator<AppBranchCreateDto>, AppBranchCreateDtoValidator>();
            services.AddTransient<IValidator<AppBranchUpdateDto>, AppBranchUpdateDtoValidator>();
            

            //dependecy injectionlar ve validaionlar buraya
        }
       
    }
}
