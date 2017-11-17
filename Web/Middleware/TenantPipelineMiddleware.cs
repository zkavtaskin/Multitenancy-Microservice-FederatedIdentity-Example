using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Owin;
using Server.Service;
using Microsoft.Owin.Builder;
using System.Collections.Generic;

namespace Web.Middleware
{
    /// <summary>
    /// Workaround for the known OpenID multitenancy issue https://github.com/aspnet/Security/issues/1179
    /// based on http://benfoster.io/blog/aspnet-core-multi-tenant-middleware-pipelines and https://weblogs.asp.net/imranbaloch/conditional-middleware-in-aspnet-core designs 
    /// </summary>
    public class TenantPipelineMiddleware
    {
        private static readonly ConcurrentDictionary<TenantContext, Lazy<Func<IDictionary<string, object>, Task>>> branches
            = new ConcurrentDictionary<TenantContext, Lazy<Func<IDictionary<string, object>, Task>>>();

        public async Task Invoke(IDictionary<string, object> env, Func<IDictionary<string, object>, Task> next, IAppBuilder rootApp, TenantContext tenantContext, Action<TenantContext, IAppBuilder> newBranchAppConfig)
        {
            if (!tenantContext.IsEmpty())
            {
                Lazy<Func<IDictionary<string, object>, Task>> branch = 
                    branches.GetOrAdd(tenantContext, new Lazy<Func<IDictionary<string, object>, Task>>(() =>
                    {
                        IAppBuilder newAppBuilderBranch = rootApp.New();
                        newBranchAppConfig(tenantContext, newAppBuilderBranch);
                        newAppBuilderBranch.Run((dic) => { return next(dic.Environment); });
                        return newAppBuilderBranch.Build();
                    }));

                await branch.Value(env);
            }
            else
            {
                await next(env);
            }
        }
    }
}