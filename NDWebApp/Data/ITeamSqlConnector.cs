using NDWebApp.Entities;
using System.Data;

namespace NDWebApp.Data
{
    public interface ITeamSqlConnector
    {
        IEnumerable<TeamEntity> GetTeams();
        IDbConnection GetDbConnection();
    }
}
