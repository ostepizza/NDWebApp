using System.Data;

namespace NDWebApp.Data
{
    public interface ISqlConnection
    {
        IDbConnection GetDbConnection();

    }
}
