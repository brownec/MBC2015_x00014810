using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MBC2015.Startup))]
namespace MBC2015
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
