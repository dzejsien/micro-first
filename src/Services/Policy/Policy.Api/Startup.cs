using Common;
using Core.Common.EventBus;
using Core.EventBus;
using Core.Messages.Commands.Policies;
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

            services.AddRawRabbit<CorrelationContext>(cfg => cfg
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("rabbit.json")
                .AddEnvironmentVariables("RABBIT_"));

            services.AddScoped<IPolicyService, PolicyService>()
                .AddTransient<IHandler, Handler>()
                .AddTransient<IBusPublisher, BusPublisher>()
                .AddTransient<IBusSubscriber, BusSubscriber>();


            services.AddTransient<ICommandHandler<CreatePolicyCommand>, CreatePolicyHandler>();
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
                .SubscribeCommand<CreatePolicyCommand>();
        }
    }
}
