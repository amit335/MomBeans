using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MomBeans.Main.Startup))]
namespace MomBeans.Main
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
