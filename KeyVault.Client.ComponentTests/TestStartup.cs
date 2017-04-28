namespace KeyVault.Client.ComponentTests
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;

    using Microsoft.Owin.Testing;
    using NUnit.Framework;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;

    public class Foo : ITokeniserService
    {
        public Task<string> Tokenise<T>(T data)
        {
            throw new NotImplementedException();
        }

        public Task<T> Detokenise<T>(string token)
        {
            throw new NotImplementedException();
        }
    }

    public class TestStartup : Startup
    {
        public void ConfigureServices(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(this.ConfigureContainer());
        }

        protected override Container ConfigureContainer()
        {
            var container = new Container();
            container.RegisterSingleton<ITokeniserService, Foo>();

            container.Verify();

            return container;
        }
    }

    [TestFixture]
    public class CardsControllerTests
    {
        [Test]
        public void When_Doing_Something()
        {
            using (var server = TestServer.Create<TestStartup>())
            {
                var result = server.HttpClient.GetAsync("http://foo/api/cards/token");
            }
        }
    }
}