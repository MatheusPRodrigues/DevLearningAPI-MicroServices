using Microsoft.Data.SqlClient;

namespace DevLearning.AuthorAPI.DataBase
{
    public class ConnectionDB
    {

        private readonly string _connectionString;

        public ConnectionDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection2");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
