using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(D2.Startup))]
namespace D2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
