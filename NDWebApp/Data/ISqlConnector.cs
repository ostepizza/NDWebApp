using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface ISqlConnector
    {
        IEnumerable<TeamEntity> GetTeams();

        TeamModel GetTeamById(int id);

        IDbConnection GetDbConnection();
    }
}
