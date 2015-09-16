using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ETest.Startup))]
namespace ETest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
