using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.MassTransitEventBus;
using Core.Messages.Commands.Payments;
using Core.Messages.Events.Pricing;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Payment.Api.Handlers;
using Payment.DataAccess.MongoDb;
using Payment.Domain;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Payment.Api
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Payment API",
                    Description = "Payment API",
                    TermsOfService = "None"
                });
            });

            //services.AddRawRabbit<CorrelationContext>(cfg => cfg
            //    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            //    .AddJsonFile("rabbit.json")
            //    .AddEnvironmentVariables("RABBIT_"));

            services.AddSingleton<IAccountService, AccountService>()
                .AddTransient<IHandler, Handler>()
                .AddTransient<IBusPublisher, Core.MassTransitEventBus.BusPublisher>();
                //.AddTransient<IBusSubscriber, BusSubscriber>();

            // todo: define "by convension"
            services.AddTransient<IEventHandler<ContributionCalculatedEvent>, ContributionCalculatedHandler>()
                .AddTransient<ICommandHandler<SendRemittanceCommand>, SendRemittanceHandler>();

            services.AddScoped<ContributionCalculatedEventConsumer>();
            services.AddScoped<SendRemittanceCommandConsumer>();


            services.AddMassTransit(x =>
            {
                x.AddConsumer<ContributionCalculatedEventConsumer>();
                x.AddConsumer<SendRemittanceCommandConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.0.75.2:5672/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, typeof(ContributionCalculatedEvent).FullName, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.LoadFrom(provider);

                    EndpointConvention.Map<ContributionCalculatedEvent>(e.InputAddress);
                });

                cfg.ReceiveEndpoint(host, typeof(SendRemittanceCommand).FullName, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.LoadFrom(provider);

                    EndpointConvention.Map<SendRemittanceCommand>(e.InputAddress);
                });
            }));

            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, MassTransitHostedService>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment API v1");
            });

            //app.UseRabbitMq()
            //    .SubscribeEvent<ContributionCalculatedEvent>()
            //    .SubscribeCommand<SendRemittanceCommand>();
        }
    }

    

    // todo: move to global core
    
}
