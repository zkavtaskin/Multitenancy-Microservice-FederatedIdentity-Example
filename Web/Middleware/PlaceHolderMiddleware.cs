using System.Threading.Tasks;
using Microsoft.Owin;

namespace Web.Middleware
{
    public class PlaceHolderMiddleware : OwinMiddleware
    {
        string name;

        public PlaceHolderMiddleware(OwinMiddleware next, string name)
        : base(next)
    {
            this.name = name;
            System.Diagnostics.Debug.WriteLine($">> {this.name} placeholder middleware constructed");
        }

        public async override Task Invoke(IOwinContext context)
        {
            System.Diagnostics.Debug.WriteLine($">> {this.name} placeholder going to call next");
            await Next.Invoke(context);
            System.Diagnostics.Debug.WriteLine($">> {this.name} placeholder done calling next");
        }
    }
}