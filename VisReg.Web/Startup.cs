using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(VisReg.Web.Startup))]

namespace VisReg.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
