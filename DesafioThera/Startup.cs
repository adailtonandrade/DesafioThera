using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DesafioThera.Startup))]
namespace DesafioThera
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
