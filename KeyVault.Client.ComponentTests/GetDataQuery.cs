namespace KeyVault.Client.ComponentTests
{
    using System.Threading.Tasks;

    using KeyVault.Client.Queries;

    internal class GetDataQuery : IGetDataQuery
    {
        public Task<string> Execute(string token)
        {
            throw new System.NotImplementedException();
        }
    }
}