namespace KeyVault.Client.ComponentTests
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;

    using Microsoft.Owin.Testing;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class Given_A_Request
    {
        [Test]
        public async void When_Tokenizing()
        {
            HttpResponseMessage result;

            using (var server = TestServer.Create<TestStartup>())
            {
                var foo = new Foo { Bar = "hello world" };
                var content = JObject.FromObject(foo);
                //await server.HttpClient.GetAsync("api/foo");
                result = await server.HttpClient.PutAsync("api/tokenise", new ObjectContent(typeof(Foo), foo, new JsonMediaTypeFormatter()));
            }

            //Assert.That(AddDataCommand.Items, Has.Count.EqualTo(1));
            //Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }
}