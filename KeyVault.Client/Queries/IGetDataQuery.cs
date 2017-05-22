namespace KeyVault.Client.Queries
{
    using System.Threading.Tasks;

    public interface IGetDataQuery
    {
        Task<string> Execute(string token);
    }
}