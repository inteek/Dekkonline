using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DekkOnlineMVC.Startup))]
namespace DekkOnlineMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
