using MySqlConnector;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public class RepairsSqlConnector : IRepairsSqlConnector
    {
        private readonly IConfiguration config;

        public RepairsSqlConnector(IConfiguration config)
        {
            this.config = config;
        }
        public IEnumerable<RepairsEntity> GetRepairs()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("SELECT RepairsId, RepairsTitle, RepairsDescription, RepairsDeadline, RepairsEnddate, UserId, TeamId, StatusId FROM repairs;", connection);
            var repairs = new List<RepairsEntity>();
            while (reader.Read())
            {
                var repair = new RepairsEntity();
                repair.RepairId = reader.GetInt32(0);
                repair.RepairTitle = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    repair.RepairDescription = reader.GetString(2);
                else repair.RepairDescription = string.Empty;

                if (!reader.IsDBNull(3))
                    repair.RepairDeadline = reader.GetDateTime(3);
                else repair.RepairDeadline = DateTime.MinValue;

                if (!reader.IsDBNull(4))
                    repair.RepairEnddate = reader.GetDateTime(4);
                else repair.RepairEnddate = DateTime.MinValue;

                repair.UserId = reader.GetString(5);

                if (!reader.IsDBNull(6))
                    repair.TeamId = reader.GetInt32(6);
                else repair.TeamId = null;
                repair.StatusId = reader.GetInt32(7);

                repairs.Add(repair);
            }
            connection.Close();

            var query = "";


            //Loop to get users name
            foreach (var repair in repairs)
            {
                System.Diagnostics.Debug.WriteLine(repair.RepairTitle);
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
                System.Diagnostics.Debug.WriteLine(repair.RepairTitle);
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
                System.Diagnostics.Debug.WriteLine(repair.RepairTitle);
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
            var query = ("SELECT RepairsId, RepairsTitle, RepairsDescription, RepairsDeadline, RepairsEnddate, UserId, TeamId, StatusId FROM repairs WHERE RepairsId = " + id + ";");
            var reader = ReadData(query, connection);
            var repair = new RepairsModel();
            while (reader.Read())
            {
                repair.RepairId = reader.GetInt32(0);
                repair.RepairTitle = reader.GetString(1);
                repair.RepairDescription = reader.GetString(2);
                if (!reader.IsDBNull(3))
                    repair.RepairDeadline = reader.GetDateTime(3);
                else repair.RepairDeadline = DateTime.MinValue;
                if (!reader.IsDBNull(4))
                    repair.RepairEnddate = reader.GetDateTime(4);
                else repair.RepairEnddate = DateTime.MinValue;
                repair.UserId = reader.GetString(5);
                if (!reader.IsDBNull(6))
                    repair.TeamId = reader.GetInt32(6);
                else repair.TeamId = null;
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
            string teamIdAsString;
            if (teamId == 0)
            {
                teamIdAsString = "NULL";
                //Duct-tape time!!!!!
                //This shit looks stupid but in theory no team Id can ever be 0 unless manually added,
                //because a new team will always look for highest number available + 1
                //So if highest number = 0 teams then new team will have ID 1
                //The form won't send Null if member isn't in any teams
                //Here we force that shit to be null to avoid issues
                //attempting to add teamId = 0
            } else
            {
                teamIdAsString = teamId.ToString();
            }

            var dateValue = repDea; //Takes supplied DateTime
            string formatDateForMySql = dateValue.ToString("yyyy-MM-dd HH:mm:ss"); //Then converts it into a format MySQL understands (or we'd all be stuck in year 0)

            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            var newRepairId = (FindHighestId() + 1);
            connection.Open();
            var query = ("INSERT INTO `repairs` (`RepairsId`, `RepairsTitle`, `RepairsDescription`, `RepairsDeadline`, `RepairsEnddate`, `UserId`, `TeamId`, `StatusId`) VALUES ('" + newRepairId + "', '" + repTitle + "', '" + repDesc + "', '" + formatDateForMySql + "', NULL, '" + repUse + "', " + teamIdAsString + ", '3');");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
            return newRepairId; //Returns ID for added suggestion so Controller can redirect to the page for it
        }

        //YOINKED from SuggestionSqlConnector
        public void UpdateRepair(int RepairId, string RepairTitle, string RepairDescription, DateTime RepairDeadline, DateTime RepairEnddate, int StatusId)
        {
            var dateValueDeadline = RepairDeadline;
            string DeadlineMySql = dateValueDeadline.ToString("yyyy-MM-dd HH:mm:ss");

            var dateValueEnddate = RepairEnddate;
            string EnddateMySql = dateValueEnddate.ToString("yyyy-MM-dd HH:mm:ss");

            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("UPDATE `repairs` SET `repairstitle` = '" + RepairTitle + "', `repairsdescription` = '" + RepairDescription + "', repairsDeadline = '" + DeadlineMySql + "', repairsenddate = '" + EnddateMySql + "', StatusId = " + StatusId + " WHERE `repairs`.`repairsid` = " + RepairId + ";");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public void UpdateStatus(int RepairId, int StatusId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("UPDATE `repairs` SET `StatusId` = '" + StatusId + "' WHERE `repairs`.`repairsid` = " + RepairId + ";");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        private int FindHighestId()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT RepairsId FROM `repairs` WHERE `RepairsId` = (SELECT MAX(RepairsId) FROM repairs);");
            var reader = ReadData(query, connection);
            int highestId = 0;
            while (reader.Read())
            {
                highestId = reader.GetInt32("RepairsId");
            }
            connection.Close();
            return highestId;
        }

        public void DeleteRepair(int repairId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("DELETE FROM repairs WHERE `repairs`.`RepairsId` = " + repairId + ";");
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public void PopulateStatusInDB()
        {
            //Highly ineffective way (probably) of making sure all statuses are present
            //Basically tries to insert the statuses every time RepairController is constructed
            //Brain is too fried to do it any other way
            //Method identical to the one in SuggestionSqlConnector
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("INSERT INTO `status` (StatusId, StatusTitle) VALUES (0, 'Under vurdering') ON DUPLICATE KEY UPDATE StatusId = 0;INSERT INTO `status` (StatusId, StatusTitle) VALUES (1, 'Godtatt') ON DUPLICATE KEY UPDATE StatusId = 1;INSERT INTO `status` (StatusId, StatusTitle) VALUES (2, 'Avslått') ON DUPLICATE KEY UPDATE StatusId = 2;INSERT INTO `status` (StatusId, StatusTitle) VALUES (3, 'Pågår') ON DUPLICATE KEY UPDATE StatusId = 3;INSERT INTO `status` (StatusId, StatusTitle) VALUES (4, 'På pause') ON DUPLICATE KEY UPDATE StatusId = 4;INSERT INTO `status` (StatusId, StatusTitle) VALUES (5, 'Ferdig') ON DUPLICATE KEY UPDATE StatusId = 5;"); //Om du er like langt til høyre som slutten på denne stringen så har du gjort noe feil i livet
            var reader = ReadData(query, connection);
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

        public IEnumerable<TeamEntity> GetTeams()
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

        public IEnumerable<StatusEntity> GetStatusList()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("Select StatusId, StatusTitle from `Status`;");
            var reader = ReadData(query, connection);
            var statusList = new List<StatusEntity>();
            while (reader.Read())
            {
                var status = new StatusEntity();
                status.StatusId = reader.GetInt32(0);
                status.StatusTitle = reader.GetString(1);

                statusList.Add(status);
            }
            connection.Close();
            return statusList;
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