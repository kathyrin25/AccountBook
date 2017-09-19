using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AccountBook.Startup))]
namespace AccountBook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
