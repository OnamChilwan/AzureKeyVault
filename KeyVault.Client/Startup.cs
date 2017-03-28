namespace KeyVault.Client
{
    using System.Web.Http;
    using KeyVault.Client.Repositories;
    using KeyVault.Client.Services;
    using Newtonsoft.Json.Serialization;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            const string ClientId = "CLIENT ID";
            const string ClientSecret = "SECRET";
            const string Uri = "VAULT URI";

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterSingleton<IKeyVaultService>(new KeyVaultService(Uri, ClientId, ClientSecret));
            container.RegisterSingleton<ITokeniserService, TokeniserService>();
            container.RegisterSingleton<IAddCardCommand, AddCardCommand>();
            container.RegisterSingleton<IGetCardQuery, GetCardQuery>();
            container.Verify();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            app.UseWebApi(config);
        }
    }
}