namespace KeyVault.Client.Queries
{
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Dapper;

    public interface IGetDataQuery
    {
        Task<string> Execute(string token);
    }

    public class SqlGetDataQuery : IGetDataQuery
    {
        private readonly string connectionString;

        public SqlGetDataQuery(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public async Task<string> Execute(string token)
        {
            const string Sql =
                "SELECT C.[Data] " +
                "FROM CardHolderData C " +
                "INNER JOIN Tokens T ON T.LinkId = C.CardHolderDataId " +
                "WHERE T.Token = @token";

            using (var conn = new SqlConnection(this.connectionString))
            {
                return await conn.QuerySingleOrDefaultAsync<string>(Sql, new { Token = token });
            }
        }
    }
}