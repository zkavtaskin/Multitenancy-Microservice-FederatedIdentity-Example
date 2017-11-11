using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Server.Core.Container;
using Server.Service;
using System.Collections.Generic;

namespace Web.Middleware
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseMultitenancy(this IAppBuilder app, WhenMiddleware whenMiddleware, MultitenancyNotifications notifications)
        {
            Func<IOwinContext, Func<Task>, Task> conditionalNext = (context, next) =>
                ServiceLocator.Resolve<MultitenancyMiddleware>(new {notifications = notifications})
                    .Invoke(context, next);

            app.Use((context, next) => whenMiddleware.Invoke(context,  conditionalNext, next));

            return app;
        }

        public static IAppBuilder UsePerTenant(this IAppBuilder app, Action<TenantContext, IAppBuilder> newBranchAppConfig)
        {
            app.Use(new Func<Func<IDictionary<string, object>, Task>, Func<IDictionary<string, object>, Task>>(
                next => (async (env) => {
                    TenantContext tenantContext = ServiceLocator.Resolve<TenantContext>();
                    await new TenantPipelineMiddleware().Invoke(env, next, app, tenantContext, newBranchAppConfig);
                }))
            );

            return app;
        }
    }
}