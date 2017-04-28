namespace KeyVault.Client.Services
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Threading.Tasks;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Queries;

    public interface ITokeniserService
    {
        Task<string> Tokenise<T>(T data);

        Task<T> Detokenise<T>(string token);
    }

    public class TokeniserService : ITokeniserService
    {
        private readonly IAddCardCommand addCardCommand;
        private readonly IGetCardQuery getCardQuery;

        public TokeniserService(
            IAddCardCommand addCardCommand,
            IGetCardQuery getCardQuery)
        {
            this.addCardCommand = addCardCommand;
            this.getCardQuery = getCardQuery;
        }

        public async Task<string> Tokenise<T>(T data)
        {
            // create a JWT here
            var token = Guid.NewGuid().ToString("D"); //Token.Create();
            string base64String;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                formatter.Serialize(stream, data);
                await stream.FlushAsync();
                stream.Position = 0;
                base64String = Convert.ToBase64String(stream.ToArray());
            }

            await this.addCardCommand.Execute(token, base64String);

            return await Task.FromResult(token);
        }

        public async Task<T> Detokenise<T>(string token)
        {
            var data = await this.getCardQuery.Execute(token);

            using (var stream = new MemoryStream(Convert.FromBase64String(data)))
            {
                var formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}