namespace KeyVault.Client
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Web.Http;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Microsoft.SqlServer.Management.AlwaysEncrypted.AzureKeyVaultProvider;
    using Newtonsoft.Json.Serialization;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public class Startup
    {
        private string clientId;
        private string clientSecret;
        private string uri;
        private string connectionString;

        public void Configuration(IAppBuilder app)
        {
            this.ReadConfiguration();

            var config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this.ConfigureContainer());

            ConfigureWebApi(config);
            this.ConfigureAlwaysEncryptedSql();

            app.UseWebApi(config);
        }

        private void ConfigureAlwaysEncryptedSql()
        {
            var azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(this.GetToken);
            var providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>();
            providers.Add(SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider);
            SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
        }

        protected virtual void ReadConfiguration()
        {
            this.clientId = ConfigurationManager.AppSettings["clientId"];
            this.clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            this.uri = ConfigurationManager.AppSettings["vaultUri"];
            this.connectionString = ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }

        protected virtual Container ConfigureContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterSingleton<IKeyVaultService>(new KeyVaultService(this.uri, this.clientId, this.clientSecret));
            container.RegisterSingleton<ITokeniserService, TokeniserService>();
            container.RegisterSingleton<IAddDataCommand>(new SqlAddDataCommand(this.connectionString));
            container.RegisterSingleton<IGetDataQuery>(new SqlGetDataQuery(this.connectionString));
            container.Verify();

            return container;
        }

        private static void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Never;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        private async Task<string> GetToken(string authority, string resource, string scope)
        {
            var context = new AuthenticationContext(authority);
            var credentials = new ClientCredential(this.clientId, this.clientSecret);
            var result = await context.AcquireTokenAsync(resource, credentials);

            return result.AccessToken;
        }
    }
}