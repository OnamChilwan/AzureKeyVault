namespace KeyVault.Client.Queries
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Dapper;

    public class SqlGetSecurityCodeQuery : IGetDataQuery
    {
        private readonly string connectionString;

        public SqlGetSecurityCodeQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<string> Execute(string token)
        {
            try
            {
                const string Sql =
                    "SELECT C.[Data] " +
                    "FROM SecurityCode C " +
                    "INNER JOIN Tokens T ON T.LinkId = C.SecurityCodeId " +
                    "WHERE T.Token = @token";

                using (var conn = new SqlConnection(this.connectionString))
                {
                    var result = await conn.QuerySingleAsync<int>("SELECT [LinkId] FROM [Tokens] WHERE Token = @token", new { token = token });
                    var data = await conn.QuerySingleOrDefaultAsync<string>("SELECT [Data] FROM SecurityCode WHERE [SecurityCodeId] = @id", new { id = result });

                    return data;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}