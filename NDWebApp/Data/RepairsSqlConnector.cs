using MySqlConnector;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public class RepairsSqlConnector : IRepairsSqlConnector
    {
        private readonly IConfiguration config;

        public RepairsSqlConnector()
        {
            this.config = config;
        }
        public IEnumerable<RepairsEntity> GetRepairs()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("SELECT RepairsId, RepairsTitle, RepairsDescription, RepairsDeadline, RepairsEnddate, RepairsUserId, TeamId, StatusId FROM repairs;", connection);
            var repairs = new List<RepairsEntity>();
            while (reader.Read())
            {
                var repair = new RepairsEntity();
                repair.RepairsId = reader.GetInt32("RepairsId");
                repair.RepairsTitle = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    repair.RepairsDescription = reader.GetString(2);
                else repair.RepairsDescription = string.Empty;
                repair.RepairsDeadline = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    repair.RepairsEnddate = reader.GetDateTime(4);
                else repair.RepairsEnddate = DateTime.MinValue;
                repair.UserId = reader.GetString(5);
                repair.TeamId = reader.GetInt32(6);
                repair.StatusId = reader.GetInt32(7);

                repairs.Add(repair);
            }
            connection.Close();

            var query = "";


            //Loop to get users name
            foreach (var repair in repairs)
            {
                System.Diagnostics.Debug.WriteLine(repair.RepairsTitle);
                connection.Open();
                query = ("SELECT empFname, empLname FROM AspNetUsers WHERE Id = '" + repair.UserId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    repair.UserFirstname = reader.GetString(0);
                    repair.UserLastname = reader.GetString(1);
                }
                connection.Close();
            }

            //Loop to get Team name
            foreach (var repair in repairs)
            {
                System.Diagnostics.Debug.WriteLine(repair.RepairsTitle);
                connection.Open();
                query = ("SELECT TeamName FROM Team WHERE TeamId = '" + repair.TeamId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    repair.TeamName = reader.GetString(0);
                }
                connection.Close();
            }

            //Loop to get Status title
            foreach (var repair in repairs)
            {
                System.Diagnostics.Debug.WriteLine(repair.RepairsTitle);
                connection.Open();
                query = ("SELECT StatusTitle FROM Status WHERE StatusId = '" + repair.StatusId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    repair.StatusName = reader.GetString(0);
                }
                connection.Close();
            }

            return repairs;


        }

        public RepairsModel GetRepairById(int id)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection")); //Specifies connection
            connection.Open(); //Opens connection
            var query = ("SELECT SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, UserId, TeamId, StatusId FROM suggestion WHERE SuggestionId = " + id + ";");
            var reader = ReadData(query, connection);
            var repair = new RepairsModel();
            while (reader.Read())
            {
                repair.RepairsId = reader.GetInt32("TeamId");
                repair.RepairsTitle = reader.GetString(1);
                repair.RepairsDescription = reader.GetString(2);
                repair.RepairsDeadline = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    repair.RepairsEnddate = reader.GetDateTime(4);
                else repair.RepairsEnddate = DateTime.MinValue;
                repair.UserId = reader.GetString(5);
                repair.TeamId = reader.GetInt32(6);
                repair.StatusId = reader.GetInt32(7);
            }
            connection.Close(); //Closes the connection

            //Takes User ID and finds a name
            connection.Open();
            query = ("SELECT `empFname`, `empLname` FROM `AspNetUsers` WHERE Id = '" + repair.UserId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                repair.UserFirstname = reader.GetString(0);
                repair.UserLastname = reader.GetString(1);
            }
            connection.Close();

            //Takes team ID and finds the team name
            connection.Open();
            query = ("SELECT `TeamName` FROM `Team` WHERE TeamId = '" + repair.TeamId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                repair.TeamName = reader.GetString(0);
            }
            connection.Close();

            //Takes status ID and finds the status name
            connection.Open();
            query = ("SELECT `StatusTitle` FROM `Status` WHERE StatusId = '" + repair.StatusId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                repair.StatusName = reader.GetString(0);
            }
            connection.Close();

            return repair;
        }

        public int CreateRepair(string repTitle, string repDesc, DateTime repDea, string repUse, int teamId)
        {
            var dateValue = repDea; //Takes supplied DateTime
            string formatDateForMySql = dateValue.ToString("yyyy-MM-dd HH:mm:ss"); //Then converts it into a format MySQL understands (or we'd all be stuck in year 0)

            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            var newRepairId = (FindHighestId() + 1);
            connection.Open();
            var query = ("INSERT INTO `repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `RepairsUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`) VALUES ('" + newRepairId + "', '" + repTitle + "', '" + repDesc + "', '" + formatDateForMySql + "', NULL, '" + repUse + "', '" + teamId + "', '0');");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
            return newRepairId; //Returns ID for added suggestion so Controller can redirect to the page for it
        }

        private int FindHighestId()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT RepairsId FROM `repairs` WHERE `RepairsId` = (SELECT MAX(SuggestionId) FROM repairs);");
            var reader = ReadData(query, connection);
            int highestId = 0;
            while (reader.Read())
            {
                highestId = reader.GetInt32("RepairsId");
            }
            connection.Close();
            return highestId;
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