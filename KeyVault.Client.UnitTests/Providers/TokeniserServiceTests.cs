namespace KeyVault.Client.UnitTests.Providers
{
    using System.Threading.Tasks;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Models;
    using KeyVault.Client.Queries;
    using KeyVault.Client.Services;
    using Moq;
    using NUnit.Framework;
    using TestStack.BDDfy;

    [TestFixture]
    public class Tokenising_Card_Holder_Details
    {
        private TokeniserService subject;
        private CardHolderData cardHolderData;
        private Mock<IAddDataCommand> addCardCommand;
        private string result;

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

        public void Given_Valid_CHD()
        {
            this.cardHolderData = new CardHolderData();
            this.addCardCommand = new Mock<IAddDataCommand>();
            this.subject = new TokeniserService(this.addCardCommand.Object, null);
        }

        public async Task When_Tokenising()
        {
            this.result = await this.subject.Tokenise(this.cardHolderData);
        }

        //public void Then_CHD_Is_Persisted()
        //{
        //    this.addCardCommand.Verify(x => x.Execute(It.IsAny<string>(), this.cardHolderData), Times.Once);
        //}

        public void And_The_Token_Is_Returned()
        {
            Assert.That(this.result, Is.Not.Null);
        }
    }

    [TestFixture]
    public class Detokenising_Card_Holder_Details
    {
        private const string Token = "Token";
        private TokeniserService subject;
        private CardHolderData result;

        [Test]
        public void Execute()
        {
            this.BDDfy();
        }

        public void Given_A_Valid_Token()
        {
            var cardQuery = new Mock<IGetDataQuery>();
            var card = new CardHolderData { CardNumber = "CardNo", NameOnCard = "Mr Thierry Henry", EndDate = "02/22" };

            //cardQuery.Setup(x => x.Execute(Token)).ReturnsAsync(card);

            this.subject = new TokeniserService(null, cardQuery.Object);
        }

        public async Task When_Detokenising()
        {
            //this.result = await this.subject.Detokenise(Token);
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
    }
}