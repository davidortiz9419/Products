using Microsoft.Owin;

[assembly: OwinStartup(typeof(Products.API.Startup))]

namespace Products.API
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}