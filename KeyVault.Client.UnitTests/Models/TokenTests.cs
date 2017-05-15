namespace KeyVault.Client.UnitTests.Models
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using KeyVault.Client.Models;
    using Microsoft.IdentityModel.Tokens;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class Given_Some_Data
    {
        private Token token;
        private JwtSecurityToken result;

        [SetUp]
        public void When_Creating_A_Token()
        {
            var data = JObject.FromObject(new Foo()).ToString();
            this.token = Token.Create(data, "hello world this is a very secure secret sssssshhhhhh", 1);
            this.result = new JwtSecurityTokenHandler().ReadJwtToken(this.token.ToString());
        }

        [Test]
        public void Then_The_Reference_Is_Set()
        {
            Assert.That(this.token.Reference, Is.Not.Null);
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
        { }
    }

    [TestFixture]
    public class Given_A_Token
    {
        private Token result;

        [SetUp]
        public void When_Instantiating()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.UserData, "foobar") };
            var claimsIdentity = new ClaimsIdentity(claims);
            var securityTokenDescriptor = new SecurityTokenDescriptor { Subject = claimsIdentity };
            var handler = new JwtSecurityTokenHandler();
            var encodedJwt = handler.CreateToken(securityTokenDescriptor);

            this.result = new Token(handler.WriteToken(encodedJwt));
        }

        [Test]
        public void Then_The_Reference_Is_Set()
        {
            Assert.That(this.result.Reference, Is.EqualTo("foobar"));
        }
    }
}