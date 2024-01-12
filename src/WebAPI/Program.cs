using System.Reflection;
using Application;
using Infrastructure;
using WebAPI.Middlewares;
using WebAPI.Services.Users;
using WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration);

    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddHTTPLogging();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.EnableAnnotations();

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
}

var app = builder.Build();
{
    app.UseHTTPLogging();

    app.UseInfrastructure();

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    app.UseRateLimiting();

    app.Run();
}