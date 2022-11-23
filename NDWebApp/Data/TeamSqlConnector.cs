using MySqlConnector;
using System.Data;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection")); //Specifies connection
            connection.Open(); //Opens connection
            var query = ("SELECT TeamId, TeamName, LeaderUserId FROM Team WHERE TeamId = " + id + ";"); //Specifies first query
            var reader = ReadData(query, connection);
            var team = new TeamModel();
            while (reader.Read())
            {
                team.TeamId = reader.GetInt32("TeamId");
                team.TeamName = reader.GetString(1);
                team.LeaderUserId = reader.GetString(2);
            }
            connection.Close(); //Closes the connection

            connection.Open(); //Reopens the connection. Reset needed or would cause errors.
            query = ("SELECT empFname, empLname FROM AspNetUsers WHERE Id = '" + team.LeaderUserId + "';"); //New query, takes LeaderUserId and uses it to find name from AspNetUsers
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                team.LeaderFirstname = reader.GetString(0); //Reader starts at 0 again, puts first name in model
                team.LeaderLastname = reader.GetString(1); //Last name in model
            }
            connection.Close();

            return team; //Returns the team model, ready for use in View
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
