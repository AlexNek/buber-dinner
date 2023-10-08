using BuberDinner.Api;
using BuberDinner.Application;

var builder = WebApplication.CreateBuilder(args);
{
    var services = builder.Services;

    services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
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
