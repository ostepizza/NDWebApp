using MySqlConnector;
using System.Data;

namespace NDWebApp.Data
{
    public class SqlConnection : ISqlConnection
    {
        private readonly IConfiguration config;

        public SqlConnection(IConfiguration config)
        {
            this.config = config;
        }

        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
        }
    }
}
