namespace KeyVault.Client.Services
{
    using System.Threading.Tasks;

    using KeyVault.Client.Models;
    using KeyVault.Client.Repositories;

    public interface ITokeniserService
    {
        Task<string> Tokenise(CardHolderData cardHolderData);

        Task<CardHolderData> Detokenise(string token);
    }

    public class TokeniserService : ITokeniserService
    {
        private readonly IAddCardCommand addCardCommand;
        private readonly IGetCardQuery getCardQuery;
        private readonly IKeyVaultService keyVaultService;

        public TokeniserService(
            IAddCardCommand addCardCommand,
            IGetCardQuery getCardQuery,
            IKeyVaultService keyVaultService)
        {
            this.addCardCommand = addCardCommand;
            this.getCardQuery = getCardQuery;
            this.keyVaultService = keyVaultService;
        }

        public async Task<string> Tokenise(CardHolderData cardHolderData)
        {
            var token = Token.Create();
            var encryptedCard = await this.keyVaultService.Encrypt(cardHolderData);

            await this.addCardCommand.Execute(token.ToString(), encryptedCard);

            return await Task.FromResult(token.ToString());
        }

        public async Task<CardHolderData> Detokenise(string token)
        {
            var encryptedCard = await this.getCardQuery.Execute(token);
            var card = await this.keyVaultService.Decrypt(encryptedCard);

            return card;
        }
    }
}