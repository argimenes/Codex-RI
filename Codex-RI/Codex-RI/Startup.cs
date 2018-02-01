using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Codex_RI.Startup))]
namespace Codex_RI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
