using System;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Web.Middleware
{
    public class WhenMiddleware
    {
        private Func<IOwinContext, Task<bool>> condition;

        public WhenMiddleware(Func<IOwinContext, Task<Boolean>> condition)
        {
            this.condition = condition;
        }

        public async Task Invoke(IOwinContext context, Func<IOwinContext, Func<Task>, Task> conditionalNext, Func<Task> next)
        {
            if (condition(context).Result)
            {
                await conditionalNext(context, next);
            }
            else
            {
                await next();
            }
        }
    }
}