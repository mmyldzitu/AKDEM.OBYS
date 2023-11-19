using AKDEM.OBYS.DataAccess.Context;
using AKDEM.OBYS.DataAccess.UnitOfWork;
using AutoMapper;
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

            });
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IUow, Uow>();
            //dependecy injectionlar ve validaionlar buraya
        }
    }
}
