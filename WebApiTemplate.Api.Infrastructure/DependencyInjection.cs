using EasyCaching.Serialization.SystemTextJson.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using WebApiTemplate.Api.Application.Interfaces.Contexts;
using WebApiTemplate.Api.Application.Interfaces.Repositories;
using WebApiTemplate.Api.Application.Interfaces.Services;
using WebApiTemplate.Api.Infrastructure.DbContexts;
using WebApiTemplate.Api.Infrastructure.Repositories;
using WebApiTemplate.Api.Infrastructure.Services;

namespace WebApiTemplate.Api.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ApplicationConnection"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            }

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<ICacheService, HybridCacheService>();
            services.AddScoped<ITodoService, TodoService>();


            #region HTTP Clients

            services.AddHttpClients(configuration);

            #endregion HTTP Clients

            #region Caching

            services.AddEasyCaching(options =>
            {
                options.UseInMemory(configuration);
                options.UseRedis(configuration);
                options.UseHybrid(configuration).WithRedisBus(configuration);
                options.WithSystemTextJson();
            });

            #endregion Caching

            #region Services

            services.AddScoped<IExternalTodosService, JsonPlaceholderTodosService>();

            #endregion Services

            #region Repositories

            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            #endregion Repositories
        }

        #region Private Methods

        private static void AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            string externalApiClientName = configuration.GetValue<string>("ExternalApi:Name");
            string externalApiBaseUrl = configuration.GetValue<string>("ExternalApi:BaseUrl");

            services.AddHttpClient(externalApiClientName, client =>
            {
                client.BaseAddress = new Uri(externalApiBaseUrl);
            }).AddPolicyHandler(GetRetryPolicy(configuration));
        }

        // https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/implement-http-call-retries-exponential-backoff-polly
        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IConfiguration configuration)
        {
            int retryCount = configuration.GetValue<int>("Http:RetryCount");
            TimeSpan medianFirstRetryDelay = TimeSpan.FromSeconds(configuration.GetValue<int>("Http:MedianFirstRetryDelayInSeconds"));

            IEnumerable<TimeSpan> delay = Backoff.DecorrelatedJitterBackoffV2(medianFirstRetryDelay, retryCount);

            return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                    .WaitAndRetryAsync(delay);
        }

        #endregion Private Methods
    }
}
