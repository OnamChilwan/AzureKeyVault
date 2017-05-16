namespace KeyVault.Client.ComponentTests
{
    using System.Web.Http;
    using KeyVault.Client.Commands;
    using KeyVault.Client.ComponentTests.Data;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public class TestStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this.ConfigureContainer());

            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }

        private Container ConfigureContainer()
        {
            const string SecurityKey = "hello world this is a very secure secret sssssshhhhhh";
            var container = new Container();
            var repo = new Repository();

            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterSingleton<IAddDataCommand>(repo);
            container.RegisterSingleton<IGetDataQuery>(repo);
            container.RegisterSingleton<ITokeniserService>(new TokeniserService(repo, repo, SecurityKey));
            container.Verify();

            return container;
        }
    }
}