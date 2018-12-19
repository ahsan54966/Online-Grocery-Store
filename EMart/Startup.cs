using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EMart.Startup))]
namespace EMart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
