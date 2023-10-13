using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Imperial.SalesOrder.Web.Startup))]
namespace Imperial.SalesOrder.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
