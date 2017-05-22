namespace KeyVault.Client.UnitTests.Controllers
{
    using System.Net.Http;
    using System.Web.Http.Results;
    using KeyVault.Client.Controllers;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;
    using Moq;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class TokenControllerTests
    {
        [Test]
        public async void When_Tokenizing_Successfully()
        {
            const string Token = "token";
            var tokeniserService = new Mock<ITokeniserService>();
            var subject = new CardTokenController(tokeniserService.Object);
            var content = JObject.FromObject(new Foo());

            tokeniserService.Setup(x => x.Tokenise(content.ToString())).ReturnsAsync(Token);
            subject.Request = new HttpRequestMessage();

            var result = await subject.Put(content) as CreatedNegotiatedContentResult<Reference>;

            Assert.That(result.Content.Value, Is.EqualTo(Token));
        }

        [Test]
        public async void When_Detokenising_A_Token_Which_Does_Not_Exist()
        {
            var subject = new CardTokenController(new Mock<ITokeniserService>().Object);
            var result = await subject.Post(new Reference { Value = "token-value" }) as NotFoundResult;

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async void When_Detokenising_A_Token_Successfully()
        {
            const string Token = "token";
            var tokeniserService = new Mock<ITokeniserService>();
            var subject = new CardTokenController(tokeniserService.Object);
            var content = JObject.FromObject(new Foo { Bar = "Arsene Wenger" });

            tokeniserService.Setup(x => x.Detokenise(Token)).ReturnsAsync(content.ToString());

            var response = await subject.Post(new Reference { Value = Token }) as OkNegotiatedContentResult<JObject>;
            var result = response.Content.ToObject<Foo>();

            Assert.That(result.Bar, Is.EqualTo("Arsene Wenger"));
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }
}