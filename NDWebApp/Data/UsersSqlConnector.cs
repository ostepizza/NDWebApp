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
    public class UsersSqlConnector : IUsersSqlConnector
    {
        private readonly IConfiguration config;

        public UsersSqlConnector(IConfiguration config)
        {
            this.config = config;
        }

        public IEnumerable<UserEntity> GetMatchingUsers(string search)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("Select Id, Email, PhoneNumber, empFname, empLname, TeamId FROM AspNetUsers WHERE Email like '%"+search+"%' OR PhoneNumber LIKE '%"+search+"%' OR empFname LIKE '%"+search+"%' OR empLname LIKE '%"+search+"%';", connection);
            //Select Id, Email, PhoneNumber, empFname, empLname, TeamId FROM AspNetUsers WHERE Email like '%Sch%' OR PhoneNumber LIKE '%Sch%' OR empFname LIKE '%Sch%' OR empLname LIKE '%Sch%';
            var users = new List<UserEntity>();
            while (reader.Read())
            {
                var user = new UserEntity();

                user.Id = reader.GetString(0);
                user.Email = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    user.Phone = reader.GetString(2);
                else user.Phone = string.Empty;
                //user.Phone = reader.GetString(2);
                user.empFname = reader.GetString(3);
                user.empLname = reader.GetString(4);
                if (!reader.IsDBNull(5))
                    user.TeamId = reader.GetInt32(5);
                else user.TeamId = null;
                

                users.Add(user);
            }
            connection.Close();

            var query = "";

            //Loop to get Team name
            foreach (var user in users)
            {
                if (user.TeamId != null)
                {
                    connection.Open();
                    query = ("SELECT TeamName FROM Team WHERE TeamId = '" + user.TeamId + "';");
                    reader = ReadData(query, connection);
                    while (reader.Read())
                    {
                        user.TeamName = reader.GetString(0);
                    }
                    connection.Close();
                }        
            }

            return users;
        }

        public UserModel GetUserById(string id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection")); //Specifies connection
            connection.Open(); //Opens connection
            var query = ("SELECT Id, Email, PhoneNumber, empFname, empLname, empNr, teamId FROM AspNetUsers WHERE Id = '" + id + "';"); //Specifies first query
            var reader = ReadData(query, connection);
            var user = new UserModel();
            while (reader.Read())
            {
                user.Id = reader.GetString(0);
                user.Email = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    user.Phone = reader.GetString(2);
                else user.Phone = string.Empty;
                user.empFname = reader.GetString(3);
                user.empLname = reader.GetString(4);
                user.empNr = reader.GetInt32(5);
                if (!reader.IsDBNull(6))
                    user.teamId = reader.GetInt32(6);
                else user.teamId = null;

            }
            connection.Close(); //Closes the connection

            connection.Open();
            query = ("SELECT TeamName FROM Team WHERE TeamId = '" + user.teamId + "';"); //Specifies first query
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                if (!reader.IsDBNull(0))
                    user.teamName = reader.GetString(0);
                else user.teamName = null;
            }
            connection.Close();

            user.AvailableTeams = GetAvailableTeams();

            return user; //Returns the team model, ready for use in View
        }

        public IEnumerable<TeamEntity> GetAvailableTeams()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("Select TeamId, TeamName from Team;");
            var reader = ReadData(query, connection);
            var availableTeams = new List<TeamEntity>();
            while (reader.Read())
            {
                var team = new TeamEntity();
                team.TeamId = reader.GetInt32("TeamId");
                team.TeamName = reader.GetString(1);

                availableTeams.Add(team);
            }
            connection.Close();
            return availableTeams;
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
