using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.MassTransitEventBus;
using MassTransit;
using MicroDemo.Api.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            //services.AddRawRabbit<CorrelationContext>(cfg => cfg
            //.SetBasePath(System.IO.Directory.GetCurrentDirectory())
            //.AddJsonFile("rabbit.json")
            //.AddEnvironmentVariables("RABBIT_"));

            services.AddTransient<IBusPublisher, Core.MassTransitEventBus.BusPublisher>();

            var apiConfig = apiSection.Get<ApiConfig>();
            services.AddRefitClient<IPaymentApi>().ConfigureHttpClient(c => c.BaseAddress = new Uri(apiConfig.PaymentApiUrl));

            //services.AddScoped<OrderConsumer>();

            //services.AddMassTransit(x =>
            //{
            //    x.AddConsumer<OrderConsumer>();
            //});

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.0.75.2:5672/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            }));

            //services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            //services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<SubmitOrder>());

            services.AddSingleton<IHostedService, MassTransitHostedService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
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
