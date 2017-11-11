using System;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Owin;
using Server.Service;
using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Server.Core.Container;
using System.Collections.Generic;
using Microsoft.Owin.BuilderProperties;

namespace Web.Middleware
{
    public class TenantPipelineMiddleware2 : OwinMiddleware
    {
        private static ConcurrentDictionary<TenantContext, Func<IDictionary<string, object>, Task>> branches
            = new ConcurrentDictionary<TenantContext, Func<IDictionary<string, object>, Task>>();

        public TenantPipelineMiddleware2(OwinMiddleware next, IAppBuilder rootApp, Action<TenantContext, IAppBuilder> newBranchAppConfig, )
            : base(next)
        {

        }

        public override Task Invoke(IOwinContext context)
        {
            return this.Invoke(context);
        }

        public async Task Invoke(IOwinContext context, Func<Task> next, IAppBuilder rootApp, Action<TenantContext, IAppBuilder> newBranchAppConfig)
        {
            TenantContext tenantContext = ServiceLocator.Resolve<TenantContext>();

            if (tenantContext.IsEmpty())
            {
                await next();
            }
            else
            {
                Func<IDictionary<string, object>, Task> tenantBranch;
                branches.TryGetValue(tenantContext, out tenantBranch);
                if(tenantBranch == null)
                {
                    tenantBranch = buildBranch(tenantContext, rootApp, newBranchAppConfig, next);
                    branches.TryAdd(tenantContext, tenantBranch);
                }

                await tenantBranch(context.Environment);
            }
        }

        public Func<IDictionary<string, object>, Task> buildBranch(TenantContext tenantContext, IAppBuilder rootApp, 
            Action<TenantContext, IAppBuilder> newBranchAppConfig, Func<Task> next)
        {
            IAppBuilder newAppBuilderBranch = rootApp.New();
            newBranchAppConfig(tenantContext, newAppBuilderBranch);
            newAppBuilderBranch.Run((ctx) => { System.Diagnostics.Debug.WriteLine($"In {tenantContext.FriendlyName} branch calling next"); return next(); });
            System.Diagnostics.Debug.WriteLine($"Completed branch creation for: {tenantContext.FriendlyName}");
            return newAppBuilderBranch.Build();
        }
    }
}