using MySqlConnector;
using System.Data;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data.Common;

namespace NDWebApp.Data
{
    public class TeamSqlConnector : ISqlConnector
    {
        private readonly IConfiguration config;

        public TeamSqlConnector(IConfiguration config)
        {
            this.config = config;
        }

        public IEnumerable<TeamEntity> GetTeams()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("Select t.TeamId, t.TeamName, t.LeaderUserId, u.Id, u.empFname, u.empLname from Team AS t JOIN AspNetUsers AS u WHERE t.LeaderUserId = u.Id;", connection);
            var teams = new List<TeamEntity>();
            while (reader.Read())
            {
                var team = new TeamEntity();
                team.TeamId = reader.GetInt32("TeamId");
                team.TeamName = reader.GetString(1);
                team.LeaderUserId = reader.GetString(2);
                team.Id = reader.GetString(3);
                team.empFname = reader.GetString(4);
                team.empLname = reader.GetString(5);

                Console.WriteLine(reader.GetString(1));
                teams.Add(team);
            }
            connection.Close();
            return teams;

        }

        public TeamModel GetTeamById(int id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT TeamId, TeamName, LeaderUserId FROM Team WHERE TeamId = " + id + ";");
            var reader = ReadData(query, connection);
            var team = new TeamModel();
            while (reader.Read())
            {
                team.TeamId = reader.GetInt32("TeamId");
                team.TeamName = reader.GetString(1);
                team.LeaderUserId = reader.GetString(2);
            }
            return team;
        }

        private MySqlDataReader ReadData(string query, MySqlConnection conn)
        {
            using var command = conn.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query;
            return command.ExecuteReader();
        }
        public IDbConnection GetDbConnection()
        {
            return new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
        }
    }
}
