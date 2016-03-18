using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IndDev.Startup))]
namespace IndDev
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
