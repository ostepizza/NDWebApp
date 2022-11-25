using MySqlConnector;
using System.Data;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.Metrics;

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

                team.TeamMemberAmount = CountTeamMembers(team.TeamId);

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

            team.TeamMemberAmount = CountTeamMembers(id);

            return team; //Returns the team model, ready for use in View
        }

        public IEnumerable<TeamMemberEntity> GetTeamMembers(int id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("Select TeamId, Id, empFname, empLname from AspNetUsers WHERE TeamId = '"+id+"';");
            var reader = ReadData(query, connection);
            var teamMembers = new List<TeamMemberEntity>();
            while (reader.Read())
            {
                var teamMember = new TeamMemberEntity();
                teamMember.TeamId = reader.GetInt32("TeamId");
                teamMember.UserId = reader.GetString(1);
                teamMember.empFname = reader.GetString(2);
                teamMember.empLname = reader.GetString(3);

                teamMembers.Add(teamMember);
            }
            connection.Close();
            return teamMembers;
        }

        public int CountTeamMembers(int id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT COUNT(TeamId) FROM `aspnetusers` WHERE `TeamId` = " + id + ";");
            var reader = ReadData(query, connection);
            int count = 0;
            while (reader.Read())
            {
                count = reader.GetInt32("COUNT(TeamId)");
            }
            connection.Close();
            return count;
        }

        public int CountTeams()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT TeamId FROM `Team` WHERE `TeamId` = (SELECT MAX(TeamId) FROM Team);");
            var reader = ReadData(query, connection);
            int highestId = 0;
            while (reader.Read())
            {
                highestId = reader.GetInt32("TeamId");
            }
            connection.Close();
            return highestId;
        }

        public void CreateTeam(string teamName, string leaderId)
        {
            //First creates a team
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            var newTeamId = (CountTeams() + 1);
            connection.Open();
            var query = ("INSERT INTO `team` (`TeamId`, `TeamName`, `LeaderUserId`) VALUES ('" + newTeamId + "', '" + teamName + "', '" + leaderId + "');");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();

            //Updates AspNetUsers to show new team for Leader
            connection.Open();
            query = ("UPDATE `AspNetUsers` SET `TeamId` = '" + newTeamId + "' WHERE `Id` = '" + leaderId + "';");
            reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public void DeleteTeam(int teamId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("DELETE FROM team WHERE `team`.`TeamId` = "+ teamId +";");
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public void UpdateTeam(int teamId, string teamName, string leaderUserId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("UPDATE `team` SET `TeamName` = '" + teamName + "', `LeaderUserId` = '" + leaderUserId + "' WHERE `team`.`TeamId` = "+teamId+";");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public IEnumerable<UserEntity> GetUsers()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("Select Id, empFname, empLname FROM AspNetUsers", connection);
            var users = new List<UserEntity>();
            while (reader.Read())
            {
                var user = new UserEntity();

                user.Id = reader.GetString(0);
                user.empFname = reader.GetString(1);
                user.empLname = reader.GetString(2);

                users.Add(user);
            }
            connection.Close();
            return users;
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
