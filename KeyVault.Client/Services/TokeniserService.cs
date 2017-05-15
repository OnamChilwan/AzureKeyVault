namespace KeyVault.Client.Services
{
    using System;
    using System.Threading.Tasks;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Models;
    using KeyVault.Client.Queries;

    public interface ITokeniserService
    {
        Task<string> Tokenise(string data);

        Task<string> Detokenise(string token);
    }

    public class TokeniserService : ITokeniserService
    {
        private readonly IAddDataCommand addDataCommand;
        private readonly IGetDataQuery getDataQuery;
        private readonly string securityKey;

        public TokeniserService(
            IAddDataCommand addDataCommand,
            IGetDataQuery getDataQuery,
            string securityKey)
        {
            this.addDataCommand = addDataCommand;
            this.getDataQuery = getDataQuery;
            this.securityKey = securityKey;
        }

        public async Task<string> Tokenise(string data)
        {
            const int NumberOfDays = 1; //injected in or should this be encapsulated in the value object?
            var token = Token.Create(data, this.securityKey, NumberOfDays);

            await this.addDataCommand.Execute(token.Reference, data);

            return await Task.FromResult(token.ToString());
        }

        public async Task<string> Detokenise(string value)
        {
            var token = new Token(value);
            var data = await this.getDataQuery.Execute(token.Reference);

            if (data != null)
            {
                return data;
            }

            throw new Exception("Invalid token");
        }
    }
}