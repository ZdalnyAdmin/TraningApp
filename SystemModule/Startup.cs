using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SystemModule.Startup))]
namespace SystemModule
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
