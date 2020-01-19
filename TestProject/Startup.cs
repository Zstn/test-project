using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TestProject.Startup))]
namespace TestProject
{
    using System.Web.Services.Description;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
