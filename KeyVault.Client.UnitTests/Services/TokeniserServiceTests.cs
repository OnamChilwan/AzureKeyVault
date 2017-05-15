namespace KeyVault.Client.UnitTests.Services
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;
    using Microsoft.IdentityModel.Tokens;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class Given_Data_To_Tokenize
    {
        private Mock<IAddDataCommand> addDataCommand;
        private string data;

        [SetUp]
        public async void When_Tokenizing()
        {
            this.data = "this is some test data";
            this.addDataCommand = new Mock<IAddDataCommand>();
            var subject = new TokeniserService(this.addDataCommand.Object, null, "hello world this is a very secure secret sssssshhhhhh");

            await subject.Tokenise(this.data);
        }

        [Test]
        public void Then_Data_Is_Persisted()
        {
            this.addDataCommand.Verify(x => x.Execute(It.IsAny<string>(), this.data), Times.Once);
        }
    }

    [TestFixture]
    public class Given_Data_To_Detokenize
    {
        private TokeniserService subject;
        private string result;
        private string token;

        [SetUp]
        public async void When_Detokenizing()
        {
            var cardQuery = new Mock<IGetDataQuery>();
            var claims = new List<Claim> { new Claim(ClaimTypes.UserData, "foobar") };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor { Subject = claimsIdentity };
            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.CreateToken(securityTokenDescriptor);
            
            cardQuery.Setup(x => x.Execute("foobar")).ReturnsAsync("hello world");

            this.token = handler.WriteToken(encodedJwt);
            this.subject = new TokeniserService(null, cardQuery.Object, "hello world this is a very secure secret sssssshhhhhh");
            this.result = await this.subject.Detokenise(this.token);
        }

        [Test]
        public void Then_Data_Is_Correct()
        {
            Assert.That(this.result, Is.EqualTo("hello world"));
        }
    }
}