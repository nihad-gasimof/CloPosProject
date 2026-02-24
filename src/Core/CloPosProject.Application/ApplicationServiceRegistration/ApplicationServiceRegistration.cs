using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Features.Commands.User;
using CloPosProject.Application.Validations.User;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Application.ApplicationServiceRegistration
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(RegisterDtoValidator).Assembly);
            services.AddFluentValidationAutoValidation();

            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));
        
        }
    }
}
