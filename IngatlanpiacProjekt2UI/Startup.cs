using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RealEstateProjectUI.Startup))]
namespace RealEstateProjectUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
