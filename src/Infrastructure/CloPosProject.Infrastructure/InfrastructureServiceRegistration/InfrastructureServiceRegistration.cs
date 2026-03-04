using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Abstract.Email;
using CloPosProject.Application.Abstract.ICloudinary;
using CloPosProject.Application.Abstract.Payment;
using CloPosProject.Application.DTOs.Authentication;
using CloPosProject.Infrastructure.Concurate.Authentication;
using CloPosProject.Infrastructure.Concurate.Cloudinary;
using CloPosProject.Infrastructure.Concurate.Email;
using CloPosProject.Infrastructure.Concurate.Report;
using CloPosProject.Infrastructure.Concurate.Reservation;
using CloPosProject.Persistence.Concurate.Payment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Infrastructure.InfrastructureServiceRegistration
{
    public static class InfrastructureServiceRegistration
    {
      
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddScoped<DailyReportJob>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddHostedService<ReservationBackgroundJob>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            _addJwtBearer(services, configuration);

            return services;
        }

        private static void _addJwtBearer(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RoleClaimType = "Role",

                    ValidIssuer = configuration["TokenOptions:Issuer"],
                    ValidAudience = configuration["TokenOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenOptions:SecurityKey"] ?? "")),
                    LifetimeValidator = (_, expired, token, _) => token != null ? expired > DateTime.UtcNow : false
                };
            });


        }
    }
}
