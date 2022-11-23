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
