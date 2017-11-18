using System;
using System.Threading.Tasks;
using Owin;
using Server.Core.Container;
using Server.Service;
using System.Collections.Generic;

namespace Web.Middleware
{
    public static class AppBuilderExtensions
    {

        public static IAppBuilder UseMultitenancy(this IAppBuilder app, MultitenancyNotifications notifications)
        {
            return app.Use((context, next) =>
            {
                ITenantNameExtractor tenantNameExtractor = ServiceLocator.Resolve<ITenantNameExtractor>(new { context });

                if (tenantNameExtractor.CanExtract())
                {
                    MultitenancyMiddleware multitenancyMiddleware = ServiceLocator.Resolve<MultitenancyMiddleware>(new { notifications });
                    return multitenancyMiddleware.Invoke(context, next);
                }

                return next();
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
    }
}