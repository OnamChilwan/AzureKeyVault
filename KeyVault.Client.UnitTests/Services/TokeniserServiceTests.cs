namespace KeyVault.Client.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Models;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;

    using Microsoft.IdentityModel.Tokens;

    using Moq;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class Tokenising_Data_Successfully
    {
        private TokeniserService subject;
        private Mock<IAddDataCommand> addDataCommand;
        private string data;
        private JwtSecurityToken result;

        [SetUp]
        public async void Given_Some_Data()
        {
            this.data = JObject.FromObject(new Foo()).ToString();
            this.addDataCommand = new Mock<IAddDataCommand>();
            this.subject = new TokeniserService(this.addDataCommand.Object, null, "hello world this is a very secure secret sssssshhhhhh");
            this.result = new JwtSecurityTokenHandler().ReadJwtToken(await this.subject.Tokenise(this.data));
        }

        [Test]
        public void Then_Data_Is_Persisted()
        {
            this.addDataCommand.Verify(x => x.Execute(It.IsAny<string>(), this.data), Times.Once);
        }

        [Test]
        public void Then_The_Token_Is_Returned()
        {
            Assert.That(this.result, Is.Not.Null);
        }

        [Test]
        public void Then_The_Reference_Header_Is_Set()
        {
            var claim = this.result.Payload.Claims.Single(x => x.Type == ClaimTypes.UserData);
            Guid value;

            Assert.That(Guid.TryParse(claim.Value, out value), Is.True);
        }

        [Test]
        public void Then_The_Expire_Header_Is_Set()
        {
            var expected = DateTime.Now.AddDays(1).Date;
            Assert.That(this.result.Payload.ValidTo, Is.EqualTo(expected));
        }

        [Test]
        public void Then_The_Issuer_Header_Is_Set()
        {
            Assert.That(this.result.Payload.Iss, Is.EqualTo("Tokenizer"));
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }

    [TestFixture]
    public class Detokenising_Data_Successfully
    {
        private TokeniserService subject;
        private string result;

        [SetUp]
        public async void Given_A_Valid_Token()
        {
            var cardQuery = new Mock<IGetDataQuery>();
            var claims = new List<Claim> { new Claim(ClaimTypes.UserData, "foobar") };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor { Subject = claimsIdentity };
            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.CreateToken(securityTokenDescriptor);
            var token = handler.WriteToken(encodedJwt);

            cardQuery.Setup(x => x.Execute("foobar")).ReturnsAsync(JObject.FromObject(new Foo { Bar = "hello world" }).ToString);

            this.subject = new TokeniserService(null, cardQuery.Object, "hello world this is a very secure secret sssssshhhhhh");

            this.result = await this.subject.Detokenise(token);
        }

        public async void When_Detokenising()
        {
        }

        public void Then_Card_Number_Is_Correct()
        {
            Assert.That(this.result.CardNumber, Is.EqualTo("CardNo"));
        }

        public void And_Card_Holder_Name_Is_Correct()
        {
            Assert.That(this.result.NameOnCard, Is.EqualTo("Mr Thierry Henry"));
        }

        public void And_End_Date_Is_Correct()
        {
            Assert.That(this.result.EndDate, Is.EqualTo("02/22"));
        }

        private class Foo
        {
            public string Bar { get; set; }
        }
    }
}