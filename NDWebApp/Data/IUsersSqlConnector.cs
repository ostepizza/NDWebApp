using Microsoft.Data.SqlClient;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface IUsersSqlConnector
    {
        IEnumerable<UserEntity> GetMatchingUsers(string search);

        IEnumerable<TeamEntity> GetAvailableTeams();

        UserModel GetUserById(string id);

        IEnumerable<SuggestionEntity> GetUserSuggestions(string id);

        IEnumerable<RepairsEntity> GetUserRepairs(string id);

        IDbConnection GetDbConnection();
    }
}
