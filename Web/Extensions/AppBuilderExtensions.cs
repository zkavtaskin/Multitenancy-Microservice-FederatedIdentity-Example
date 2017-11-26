using System;
using System.Threading.Tasks;
using Owin;
using Server.Core.Container;
using Server.Service;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Owin.BuilderProperties;
using Web.Middleware;

namespace Web.Extensions
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder UseMultitenancy(this IAppBuilder app, MultitenancyNotifications notifications)
        {
            return app.Use((context, next) =>
            {
                MultitenancyMiddleware multitenancyMiddleware = ServiceLocator.Resolve<MultitenancyMiddleware>(new { next, notifications });
                return multitenancyMiddleware.Invoke(context);
            });
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

        public static IAppBuilder OnDispose(this IAppBuilder app, Action doOnDispose)
        {
            AppProperties properties = new AppProperties(app.Properties);
            CancellationToken token = properties.OnAppDisposing;
            if (token != CancellationToken.None)
            {
                token.Register(doOnDispose);
            }
            return app;
        }
    }
}