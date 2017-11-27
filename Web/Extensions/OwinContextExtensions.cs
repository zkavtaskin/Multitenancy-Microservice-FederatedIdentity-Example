using System;
using System.Threading.Tasks;
using Owin;
using Server.Core.Container;
using Server.Service;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Owin;
using Microsoft.Owin.BuilderProperties;
using Web.Middleware;

namespace Web.Extensions
{
    public static class OwinContextExtensions
    {
        static string tenantContextKey = "tenantcontext";

        public static TenantContext GetTenantContext(this IOwinContext context)
        {
            if (context.Environment.ContainsKey(tenantContextKey))
            {
                return (TenantContext)context.Environment[tenantContextKey];
            }
            return null;
        }

        public static void SetTenantContext(this IOwinContext context, TenantContext tenantContext)
        {
            context.Environment.Add(tenantContextKey, tenantContext);
        }

    }
}