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

        IDbConnection GetDbConnection();
    }
}
