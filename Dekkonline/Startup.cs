using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DekkOnline.Startup))]
namespace DekkOnline
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
