using CloPosProject.Application.Abstract.Authentication;
using CloPosProject.Application.Abstract.Payment;
using CloPosProject.Application.ApplicationServiceRegistration;
using CloPosProject.Domain.Entities;
using CloPosProject.Infrastructure.Concurate.Authentication;
using CloPosProject.Infrastructure.Concurate.Report;
using CloPosProject.Infrastructure.InfrastructureServiceRegistration;
using CloPosProject.Persistence.Concurate.Authentication;
using CloPosProject.Persistence.Concurate.Payment;
using CloPosProject.Persistence.Contexts;
using CloPosProject.Persistence.PersistenceServiceRegistration;
using CloPosProject.WebApi.Middleware.GlobalExceptionHandling;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
}); ;

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(configuration);
builder.Services.AddInfrastructureServices(configuration);
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        }));
builder.Services.AddHangfireServer();

builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CloPos Project", Version = "v1" });

    // Swagger üçün Authorize button
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();
app.UseRouting();
// Configure the HTTP request pipeline.

    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});
// Recurring Jobs
RecurringJob.AddOrUpdate<DailyReportJob>(
    "daily-report",
    job => job.GenerateYesterdayReport(),
    "0 0 * * *" // Hər gün saat 00:00-da
);
app.UseMiddleware<GlobalExceptionHandling>();
app.UseHttpsRedirection();

app.UseCors("DefaultCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Production-da düzgün authentication yoxlaması olmalıdır
        return true; // Development üçün
    }
}


//asdasd