using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Events.Policies;
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

            services.AddRawRabbit<CorrelationContext>(cfg => cfg
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("rabbit.json")
                .AddEnvironmentVariables("RABBIT_"));

            services.AddScoped<ITarrifService, TarrifService>();
            services.AddScoped<IPricingService, PricingService>()
                .AddTransient<IHandler, Handler>()
                .AddTransient<IBusPublisher, BusPublisher>()
                .AddTransient<IBusSubscriber, BusSubscriber>()
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

            app.UseRabbitMq()
                .SubscribeEvent<PolicyCreatedEvent>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pricing API");
            });
        }
    }
}
