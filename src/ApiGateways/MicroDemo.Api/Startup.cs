using Common;
using Core.EventBus;
using MicroDemo.Api.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.vNext;
using Refit;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace MicroDemo.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var apiSection = Configuration.GetSection("Apis");
            
            services.Configure<ApiConfig>(apiSection);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "MicroDemo API Gateway v1",
                    Description = "MicroDemo API Gateway v1",
                    TermsOfService = "None"
                });
            });

            services.AddRawRabbit<CorrelationContext>(cfg => cfg
            .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            .AddJsonFile("rabbit.json")
            .AddEnvironmentVariables("RABBIT_"));

            services.AddTransient<IBusPublisher, BusPublisher>();

            var apiConfig = apiSection.Get<ApiConfig>();
            services.AddRefitClient<IPaymentApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfig.PaymentApiUrl));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroDemo API Gateway v1");
            });
        }
    }
}
