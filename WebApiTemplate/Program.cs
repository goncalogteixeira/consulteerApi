using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using WebApiTemplate.Api.Application;
using WebApiTemplate.Api.Application.DTOs.Settings;
using WebApiTemplate.Api.Application.Interfaces.Services;
using WebApiTemplate.Api.Filters;
using WebApiTemplate.Api.Infrastructure;
using WebApiTemplate.Api.Services;

IConfigurationRoot configuration = new ConfigurationBuilder()
                                                   .AddJsonFile("appsettings.json")
                                                   .Build();

Log.Logger = new LoggerConfiguration()
                 .ReadFrom.Configuration(configuration)
                 .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ExternalApiSettings>(configuration.GetSection("ExternalApi"));
builder.Services.AddApplication(configuration);
builder.Services.AddInfrastructure(configuration);

builder.Services.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();

builder.Services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter()));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiTemplate", Version = "v1" });
});

builder.Services.AddApplicationInsightsTelemetry(configuration["APPINSIGHTS_CONNECTIONSTRING"]);


builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

Log.Information("Starting web host");
   // var app = Host.CreateDefaultBuilder(args)
   //.UseSerilog().Build();
 

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiTemplate v1"));
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseRouting();



//app.UseAuthentication();
//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
