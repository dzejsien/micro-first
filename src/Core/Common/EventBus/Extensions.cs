using Core.Common.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Common.EventBus
{
    public static class Extensions
    {
        public static IBusSubscriber UseRabbitMq(this IApplicationBuilder app)
        {
            return app.ApplicationServices.GetService<IBusSubscriber>();
        }
    }
}
