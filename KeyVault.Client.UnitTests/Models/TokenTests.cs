namespace KeyVault.Client.UnitTests.Models
{
    using System;
    using KeyVault.Client.Models;
    using NUnit.Framework;

    [TestFixture]
    public class TokenTests
    {
        [Test]
        public void When_Generating_A_Valid_Token()
        {
            var subject = Token.Create().ToString();
            Assert.DoesNotThrow(() => Guid.Parse(subject)); //include timestamp too?
        }
    }
}