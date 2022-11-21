using NDWebApp.Entities;
using System.Data;

namespace NDWebApp.Data
{
    public interface ISqlConnector
    {
        IEnumerable<TeamEntity> GetTeams();
        IDbConnection GetDbConnection();
    }
}
