using AKDEM.OBYS.Business.DependencyResolvers;
using AKDEM.OBYS.Business.Helpers;
using AKDEM.OBYS.UI.Mappings;
using AKDEM.OBYS.UI.Models;
using AKDEM.OBYS.UI.ValidationRules;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEM.OBYS.UI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDependencies(Configuration);
            services.AddTransient<IValidator<AppBranchCreateModel>, AppBranchCreateModelValidator>();
            services.AddTransient<IValidator<AppTeacherUpdateModel>, AppTeacherUpdateModelValidator>();
            services.AddTransient<IValidator<AppStudentCreateModel>, AppStudentCreateModelValidator>();
            services.AddTransient<IValidator<AppTeacherCreateModel>, AppTeacherCreateModelValidator>();
            services.AddTransient<IValidator<AppSessionCreateModel>, AppSessionCreateModelValidator>();
            services.AddTransient<IValidator<UpdatePasswordModel>, UpdatePasswordModelValidator>();
            services.AddTransient<IValidator<AppScheduleCreateModel>, AppScheduleCreateModelValidator>();
            services.AddTransient<IValidator<AppScheduleDetailCreateModel>, AppScheduleDetailCreateModelValidator>();
            services.AddTransient<IValidator<MergeBranchModel>, MergeBranchModelValidator>();
            
            services.AddTransient<IValidator<AppStudentUpdateModel>, AppStudentUpdateModelValidator>();
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt => {
        opt.Cookie.Name = "AkdemCookie";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Strict;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        opt.ExpireTimeSpan = TimeSpan.FromDays(20);
        opt.LoginPath = new PathString("/Account/SignIn");
        opt.LogoutPath = new PathString("/Account/LogOut");
        opt.AccessDeniedPath = new PathString("/Account/AccessDeniedPath");


    });
            
            services.AddControllersWithViews();

            var profiles = ProfileHelper.GetProfiles();

            profiles.Add(new AppBranchProfile());
            profiles.Add(new AppTeacherProfile());
            profiles.Add(new AppStudentProfile());


            var configuration = new MapperConfiguration(opt => {
                opt.AddProfiles(profiles);
            });
            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
