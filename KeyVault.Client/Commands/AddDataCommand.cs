namespace KeyVault.Client.Commands
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Dapper;

    public interface IAddDataCommand
    {
        Task Execute(string token, string data);
    }

    public class SqlAddDataCommand : IAddDataCommand
    {
        private readonly string connectionString;

        public SqlAddDataCommand(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task Execute(string token, string data)
        {
            using (var conn = new SqlConnection(this.connectionString))
            {
                await conn.OpenAsync();
                var id = await InsertData(conn, data);
                await InsertToken(conn, token, id);
            }
        }

        private static async Task<int> InsertData(IDbConnection conn, string data)
        {
            const string Sql =
                "INSERT INTO [CardHolderData] ([Data]) " + // TODO: rename table
                "VALUES (@data); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";

            return await conn.QueryFirstAsync<int>(Sql, new { data = data });
        }

        private static async Task InsertToken(IDbConnection conn, string token, int linkId)
        {
            const string Sql =
                "INSERT INTO [Tokens] (Token,LinkId) " +
                "VALUES (@token,@linkId)";

            await conn.ExecuteAsync(Sql, new { token = token, linkId = linkId });
        }
    }
}