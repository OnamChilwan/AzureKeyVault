namespace KeyVault.Client.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KeyVault.Client.Models;

    public interface IAddCardCommand
    {
        Task Execute(string token, EncryptedCard card);
    }

    public interface IGetCardQuery
    {
        Task<EncryptedCard> Execute(string token);
    }

    public class GetCardQuery : IGetCardQuery
    {
        public async Task<EncryptedCard> Execute(string token)
        {
            return await Task.FromResult(Respository.Cards[token]);
        }
    }

    public class AddCardCommand : IAddCardCommand
    {
        public async Task Execute(string token, EncryptedCard card)
        {
            await Task.Run(() => Respository.Cards.Add(token, card));
        }
    }

    public static class Respository
    {
        public static readonly Dictionary<string, EncryptedCard> Cards;

        static Respository()
        {
            Cards = new Dictionary<string, EncryptedCard>();
        }
    }
}