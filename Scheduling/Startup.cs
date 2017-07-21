using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Scheduling.Startup))]
namespace Scheduling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
