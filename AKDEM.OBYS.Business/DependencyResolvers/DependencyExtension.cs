using AKDEM.OBYS.Business.Managers;
using AKDEM.OBYS.Business.Mappings;
using AKDEM.OBYS.Business.Services;
using AKDEM.OBYS.Business.ValidationRules.AppLesson;
using AKDEM.OBYS.Business.ValidationRules.AppSession;
using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AKDEM.OBYS.Dto.AppLessonDtos;
using AKDEM.OBYS.Dto.AppSessionDtos;
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

           var mapperConfiguration =new MapperConfiguration(opt =>
            {
                opt.AddProfile(new AppSessionProfile());
                opt.AddProfile(new AppLessonProfile());
            });
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);


            services.AddScoped<IUow, Uow>();
            services.AddScoped<IAppSessionService, AppSessionManager>();

            services.AddTransient<IValidator<AppLessonCreateDto>, AppLessonCreateDtoValidator>();
            services.AddTransient<IValidator<AppSessionCreateDto>, AppSessionCreateDtoValidator>();
            services.AddTransient<IValidator<AppSessionUpdateDto>, AppSessionUpdateDtoValidator>();
            //dependecy injectionlar ve validaionlar buraya
        }
    }
}
