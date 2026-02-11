using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Commands;
using FluentValidation;
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
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly));
        
        }
    }
}
