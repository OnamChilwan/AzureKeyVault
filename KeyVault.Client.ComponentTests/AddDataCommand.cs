namespace KeyVault.Client.ComponentTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using KeyVault.Client.Commands;

    internal class AddDataCommand : IAddDataCommand
    {
        public Task Execute(string token, string data)
        {
            Items.Add(token, data);

            return Task.CompletedTask;
        }

        public static Dictionary<string, string> Items { get; }
    }
}