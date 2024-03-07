using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreManager.Data.Mapper;
using BookStoreManager.Data.Repo.imp;
using BookStoreManager.Data.Repo.intt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStoreManager.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BookStoreManagerDb")));
            // options.UseNpgsql(configuration.GetConnectionString("BookStoreManagerDb")));
            // options.UseInMemoryDatabase("BOOK"));
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddScoped<IBookRepo, BookRepo>();
            
            
            return services;
        }
    }
}