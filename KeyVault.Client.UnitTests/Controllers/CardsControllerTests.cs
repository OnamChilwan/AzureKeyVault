namespace KeyVault.Client.UnitTests.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http.Results;
    using KeyVault.Client.Controllers;
    using KeyVault.Client.Models;
    using KeyVault.Client.Services;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class CardsControllerTests
    {
        [Test]
        public async Task When_Adding_A_Card_Successfully()
        {
            var tokeniserService = new Mock<ITokeniserService>();
            var subject = new TokenController(tokeniserService.Object);
            var card = new CardHolderData { CardNumber = "1234", NameOnCard = "Arsene Wenger", EndDate = "02/20" };
            var token = "token";

            tokeniserService.Setup(x => x.Tokenise(card)).ReturnsAsync(token);
            subject.Request = new HttpRequestMessage();
            subject.Request.RequestUri = new Uri("http://some-host/");

            //var result = await subject.Post(card) as CreatedNegotiatedContentResult<CardHolderData>;

            //Assert.That(result, Is.Not.Null);
            //Assert.That(result.Location.ToString(), Is.EqualTo("http://some-host/token")); 
        }

        [Test]
        public async Task When_Providing_A_Token_Which_Does_Not_Exist()
        {
            var tokeniserService = new Mock<ITokeniserService>();
            var subject = new TokenController(tokeniserService.Object);
            //var result = await subject.Get("token-value") as NotFoundResult;

            //Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task When_Providing_A_Token_Which_Does_Exist_And_Valid_CHD()
        {
            var tokeniserService = new Mock<ITokeniserService>();
            var subject = new TokenController(tokeniserService.Object);
            var card = new CardHolderData { CardNumber = "1234", NameOnCard = "Arsene Wenger", EndDate = "02/20" };

            //tokeniserService.Setup(x => x.Detokenise("token-value")).ReturnsAsync(card);

            //var result = await subject.Get("token-value") as OkNegotiatedContentResult<CardHolderData>;

            //Assert.That(result, Is.Not.Null);
        }
    }
}