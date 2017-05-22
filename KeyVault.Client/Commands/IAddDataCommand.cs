namespace KeyVault.Client.Commands
{
    using System.Threading.Tasks;

    public interface IAddDataCommand
    {
        Task Execute(string token, string data);
    }
}