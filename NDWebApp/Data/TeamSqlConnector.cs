using MySqlConnector;
using System.Data;
using NDWebApp.Entities;
using System.Data.Common;

namespace NDWebApp.Data
{
    public class TeamSqlConnector : ITeamSqlConnector
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
            var reader = ReadData("Select TeamId, TeamName, LeaderUserId from Team;", connection);
            var teams = new List<TeamEntity>();
            while (reader.Read())
            {
                var team = new TeamEntity();
                team.TeamId = reader.GetInt32("TeamId");
                team.TeamName = reader.GetString(1);
                team.LeaderUserId = reader.GetString(2);

                Console.WriteLine(reader.GetString(1));
                teams.Add(team);
            }
            connection.Close();
            return teams;

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
