using CloPosProject.Application.Abstract.Ai;
using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Abstract.Category;
using CloPosProject.Application.Abstract.Ingredient;
using CloPosProject.Application.Abstract.MenuItem;
using CloPosProject.Application.Abstract.Order;
using CloPosProject.Application.Abstract.Report;
using CloPosProject.Application.Abstract.Reservation;
using CloPosProject.Application.Abstract.Table;
using CloPosProject.Domain.Entities;
using CloPosProject.Infrastructure.Concurate.Ai;
using CloPosProject.Persistence.Concurate.Authentication;
using CloPosProject.Persistence.Concurate.Category;
using CloPosProject.Persistence.Concurate.Ingredient;
using CloPosProject.Persistence.Concurate.MenuItem;
using CloPosProject.Persistence.Concurate.Order;
using CloPosProject.Persistence.Concurate.Report;
using CloPosProject.Persistence.Concurate.Reservation;
using CloPosProject.Persistence.Concurate.Table;
using CloPosProject.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloPosProject.Persistence.PersistenceServiceRegistration
{
    public static class PersistenceServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
           services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IAdminAIService, AdminAiService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 6;
                opt.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IMenuItemService, MenuItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITableService, TableService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IReportService, ReportService>();
        }
    }
}
