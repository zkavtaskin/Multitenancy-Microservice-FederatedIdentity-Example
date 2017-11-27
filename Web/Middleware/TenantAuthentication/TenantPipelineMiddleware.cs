using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Owin;
using Server.Service;
using Microsoft.Owin.Builder;
using System.Collections.Generic;
using Microsoft.Owin;
using Server.Core.Container;

namespace Web.Middleware
{
    /// <summary>
    /// Workaround for the known OpenID multitenancy issue https://github.com/aspnet/Security/issues/1179
    /// based on http://benfoster.io/blog/aspnet-core-multi-tenant-middleware-pipelines and https://weblogs.asp.net/imranbaloch/conditional-middleware-in-aspnet-core designs 
    /// </summary>
    public class TenantPipelineMiddleware : OwinMiddleware
    {
        readonly IAppBuilder rootApp;
        readonly Action<TenantContext, IAppBuilder> newBranchAppConfig;
        readonly ConcurrentDictionary<TenantContext, Lazy<Func<IDictionary<string, object>, Task>>> branches;

        public TenantPipelineMiddleware(OwinMiddleware next, IAppBuilder rootApp, Action<TenantContext, IAppBuilder> newBranchAppConfig)
            : base(next)
        {
            this.rootApp = rootApp;
            this.newBranchAppConfig = newBranchAppConfig;
            this.branches = new ConcurrentDictionary<TenantContext, Lazy<Func<IDictionary<string, object>, Task>>>();
        }

        public override async Task Invoke(IOwinContext context)
        {
            object value;
            context.Environment.TryGetValue("tenantcontext", out value);
            TenantContext tenantContext = value as TenantContext;

            if (tenantContext == null || tenantContext.IsEmpty())
            {
                await this.Next.Invoke(context);
                return;
            }

            Lazy<Func<IDictionary<string, object>, Task>> branch = 
                branches.GetOrAdd(tenantContext, new Lazy<Func<IDictionary<string, object>, Task>>(() =>
                {
                    IAppBuilder newAppBuilderBranch = rootApp.New();
                    newBranchAppConfig(tenantContext, newAppBuilderBranch);
                    newAppBuilderBranch.Run((oc) => this.Next.Invoke(oc));
                    return newAppBuilderBranch.Build();
                }));

            await branch.Value(context.Environment);
        }
    }
}