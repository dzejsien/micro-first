﻿using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.MassTransitEventBus;
using Core.Messages.Commands.Policies;
using GreenPipes;
using MassTransit;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Policy.Api.Handlers;
using Policy.DataAccess.SqlServer;
using Policy.Domain;
using RawRabbit.vNext;
using System;

namespace Policy.Api
{
    public class Startup
    {
        private const string PolicyDatabaseName = "PolicyDatabase";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<PolicyContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString(PolicyDatabaseName)));

            //services.AddRawRabbit<CorrelationContext>(cfg => cfg
            //    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
            //    .AddJsonFile("rabbit.json")
            //    .AddEnvironmentVariables("RABBIT_"));

            services.AddScoped<IPolicyService, PolicyService>()
                .AddTransient<IHandler, Handler>()
                .AddTransient<IBusPublisher, Core.MassTransitEventBus.BusPublisher>();
                //.AddTransient<IBusSubscriber, Core.EventBus.BusSubscriber>();


            services.AddTransient<ICommandHandler<CreatePolicyCommand>, CreatePolicyHandler>();

            services.AddScoped<CreatePolicyConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<CreatePolicyConsumer>();
            });

            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://10.0.75.2:5672/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ReceiveEndpoint(host, typeof(CreatePolicyCommand).FullName, e =>
                {
                    e.PrefetchCount = 16;
                    e.UseMessageRetry(x => x.Interval(2, 100));
                    e.LoadFrom(provider);

                    EndpointConvention.Map<CreatePolicyCommand>(e.InputAddress);
                });
            }));

            //services.AddSingleton<IPublishEndpoint>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<ISendEndpointProvider>(provider => provider.GetRequiredService<IBusControl>());
            //services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());

            //services.AddScoped(provider => provider.GetRequiredService<IBus>().CreateRequestClient<SubmitOrder>());

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
            //    .SubscribeCommand<CreatePolicyCommand>();
        }
    }
}
