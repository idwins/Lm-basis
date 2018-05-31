using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestRazor.Startup))]
namespace TestRazor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
