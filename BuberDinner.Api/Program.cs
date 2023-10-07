using BuberDinner.Api;
using BuberDinner.Application;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
                builder.Services.AddSwaggerGen(option =>
                    {
                        option.SwaggerDoc("v1", new OpenApiInfo { Title = "BiberDinner API", Version = "v1" });
                        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                                                                   {
                                                                       In = ParameterLocation.Header,
                                                                       Description = "Please enter a valid token",
                                                                       Name = "Authorization",
                                                                       Type = SecuritySchemeType.Http,
                                                                       BearerFormat = "JWT",
                                                                       Scheme = "Bearer"
                                                                   });
                        option.AddSecurityRequirement(new OpenApiSecurityRequirement
                                                          {
                                                              {
                                                                  new OpenApiSecurityScheme
                                                                      {
                                                                          Reference = new OpenApiReference
                                                                                          {
                                                                                              Type=ReferenceType.SecurityScheme,
                                                                                              Id="Bearer"
                                                                                          }
                                                                      },
                                                                  new string[]{}
                                                              }
                                                          });
                    });
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.UseAuthentication();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
    }
    app.UseAuthorization();
    app.MapControllers();
   
    app.Run();
}
