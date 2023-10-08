using BuberDinner.Api.Common.Errors;
using BuberDinner.Api.Common.Mapping;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;

namespace BuberDinner.Api;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton<ProblemDetailsFactory, BuberDinnerProblemDetailsFactory>();
        services.AddMappings();
        services.AddSwaggerGen(
            option =>
                {
                    option.SwaggerDoc("v1", new OpenApiInfo { Title = "BiberDinner API", Version = "v1" });
                    option.AddSecurityDefinition(
                        "Bearer",
                        new OpenApiSecurityScheme
                            {
                                In = ParameterLocation.Header,
                                Description = "Please enter a valid token",
                                Name = "Authorization",
                                Type = SecuritySchemeType.Http,
                                BearerFormat = "JWT",
                                Scheme = "Bearer"
                            });
                    option.AddSecurityRequirement(
                        new OpenApiSecurityRequirement
                            {
                                {
                                    new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
                                    new string[] { }
                                }
                            });
                });
        return services;
    }
}