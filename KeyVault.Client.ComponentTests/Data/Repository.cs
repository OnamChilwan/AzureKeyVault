namespace KeyVault.Client.ComponentTests.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using KeyVault.Client.Commands;
    using KeyVault.Client.Queries;

    public class Repository : IAddDataCommand, IGetDataQuery
    {
        static Repository()
        {
            Data = new Dictionary<string, string>();
        }

        public Task Execute(string token, string data)
        {
            Data.Add(token, data);

            return Task.CompletedTask;
        }

        public Task<string> Execute(string token)
        {
            string value;

            if (Data.TryGetValue(token, out value))
            {
                return Task.FromResult(value);
            }

            return null;
        }

        public static Dictionary<string, string> Data { get; }
    }
}