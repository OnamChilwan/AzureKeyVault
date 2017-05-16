namespace KeyVault.Client.ComponentTests.Scenarios
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using KeyVault.Client.ComponentTests.Data;
    using KeyVault.Client.Models;
    using Microsoft.Owin.Testing;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [TestFixture]
    public class TokenizerSucceeds
    {
        private ObjectContent content;
        private HttpResponseMessage result;
        private Reference response;

        void Given_A_Request()
        {
            this.content = new ObjectContent(typeof(Foo), new Foo { Bar = "testing" }, new JsonMediaTypeFormatter());
        }

        async void When_Tokenizing()
        {
            using (var server = TestServer.Create<TestStartup>())
            {
                this.result = await server.HttpClient.PutAsync(@"api/tokenise", content);
                var obj = JObject.Parse(await result.Content.ReadAsStringAsync());
                this.response = obj.ToObject<Reference>();
            }
        }

        void Then_A_Created_Response_Is_Returned()
        {
            Assert.That(this.result.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        void Then_The_Token_Is_Returned()
        {
            Assert.That(this.response.Value, Is.Not.Null);
        }

        void Then_The_Reference_Is_Saved()
        {
            Assert.That(Repository.Data, Has.Count.EqualTo(1));
        }

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }
}