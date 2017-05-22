namespace KeyVault.Client.Commands
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using Dapper;

    public class SqlAddSecurityCodeCommand : IAddDataCommand
    {
        private readonly string connectionString;

        public SqlAddSecurityCodeCommand(string connectionString)
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
            try
            {
                const string Sql =
                    "INSERT INTO [SecurityCode] ([Data]) " +
                    "VALUES (@data); " +
                    "SELECT CAST(SCOPE_IDENTITY() as int)";

                return await conn.QueryFirstAsync<int>(Sql, new { data = data });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static async Task InsertToken(IDbConnection conn, string token, int linkId)
        {
            try
            {
                const string Sql =
                    "INSERT INTO [Tokens] (Token,LinkId) " +
                    "VALUES (@token,@linkId)";

                await conn.ExecuteAsync(Sql, new { token = token, linkId = linkId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}