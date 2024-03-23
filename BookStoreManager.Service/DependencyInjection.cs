using System.Text;
using BookStoreManager.Data.Repo.imp;
using BookStoreManager.Data.Repo.intt;
using BookStoreManager.Service.Authentication.Implementation;
using BookStoreManager.Service.Authentication.Interface;
using BookStoreManager.Service.Service.Implementation;
using BookStoreManager.Service.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookStoreManager.Services
{
    public static class DependencyInjection
    {
       
        public static IServiceCollection AddApplication(
            
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
             var JwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, JwtSettings);
            services.AddSingleton(Options.Create(JwtSettings));
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = JwtSettings.Issuer,
                ValidAudience = JwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(JwtSettings.Secrets))

            });

        //policy for admin authorization
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));

                options.AddPolicy("SuperAdmin", policy =>
                    policy.RequireRole("SuperAdmin"));

                options.AddPolicy("AdminOrSuperAdmin", policy =>
                    policy.RequireRole("Admin", "SuperAdmin"));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "127.0.0.1:6379";
                options.InstanceName = "BookStoreManager";
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Set the session timeout duration
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddHttpContextAccessor();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookService, BookService>();   
            services.AddScoped<EncryptionService>();         
            return services;
        }

       
                    


    }
}