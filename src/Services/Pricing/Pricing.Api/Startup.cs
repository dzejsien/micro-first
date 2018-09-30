using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.MassTransitEventBus;
using Core.Messages.Events.Policies;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pricing.Api.Handlers;
using Pricing.DataAccess.PostgreSql;
using Pricing.Domain;
using RawRabbit.vNext;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Pricing.Api
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

            // Use a PostgreSQL database
            var sqlConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");

            services.AddDbContext<DomainModelPostgreSqlContext>(options =>
                options.UseNpgsql(
                    sqlConnectionString,
                    b => b.MigrationsAssembly("Pricing.DataAccess.PostgreSql")
                )
            );

            //services.AddRawRabbit<CorrelationContext>(cfg => cfg
            //    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            //    .AddJsonFile("rabbit.json")
            //    .AddEnvironmentVariables("RABBIT_"));

            services.AddScoped<ITarrifService, TarrifService>();
            services.AddScoped<IPricingService, PricingService>()
                .AddTransient<IHandler, Handler>()
                .AddTransient<IBusPublisher, Core.MassTransitEventBus.BusPublisher>()
                //.AddTransient<IBusSubscriber, BusSubscriber>()
                .AddTransient<IEventHandler<PolicyCreatedEvent>, PolicyCreatedHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Pricing API",
                    Description = "Pricing API",
                    TermsOfService = "None",
                    Contact = new Contact() { Name = "John Doe", Email = "john.doe@john.doe.pl" }
                });
            });

            services.AddScoped<PolicyCreatedEventConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PolicyCreatedEventConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.0.75.2:5672/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, typeof(PolicyCreatedEvent).FullName, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.LoadFrom(provider);

                    EndpointConvention.Map<PolicyCreatedEvent>(e.InputAddress);
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

            //app.UseRabbitMq()
            //    .SubscribeEvent<PolicyCreatedEvent>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pricing API");
            });
        }
    }
}
