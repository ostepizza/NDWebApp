using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface ISqlConnector
    {
        IEnumerable<TeamEntity> GetTeams();

        TeamModel GetTeamById(int id);

        void CreateTeam(string teamName, string leaderId);

        void DeleteTeam(int teamId);

        void UpdateTeam(int teamId, string teamName, string leaderUserId);

        IEnumerable<TeamMemberEntity> GetTeamMembers(int id);

        IEnumerable<UserEntity> GetUsers();

        IDbConnection GetDbConnection();
    }
}
