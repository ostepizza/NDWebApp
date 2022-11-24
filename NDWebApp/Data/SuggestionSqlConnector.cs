using MySqlConnector;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public class SuggestionSqlConnector : ISuggestionConnector
    {
        private readonly IConfiguration config;

        public SuggestionSqlConnector(IConfiguration config)
        {
            this.config = config;
        }
        public IEnumerable<SuggestionEntity> GetSuggestions()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var reader = ReadData("SELECT SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, SuggestedUserId, ResponsibleUserId, TeamId, StatusId FROM Suggestion;", connection);
            var suggestions = new List<SuggestionEntity>();
            while (reader.Read())
            {
                var suggestion = new SuggestionEntity();
                suggestion.SuggestionId = reader.GetInt32("SuggestionId");
                suggestion.SuggestionTitle = reader.GetString(1);
                if (!reader.IsDBNull(2))
                    suggestion.SuggestionDescription = reader.GetString(2);
                else suggestion.SuggestionDescription = string.Empty;
                suggestion.SuggestionDeadline = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    suggestion.SuggestionEnddate = reader.GetDateTime(4);
                else suggestion.SuggestionEnddate = DateTime.MinValue;
                //suggestion.SuggestionEnddate = reader.GetDateTime(4);
                suggestion.SuggestedUserId = reader.GetString(5);
                suggestion.ResponsibleUserId = reader.GetString(6);
                suggestion.TeamId = reader.GetInt32(7);
                suggestion.StatusId = reader.GetInt32(8);

                suggestions.Add(suggestion); //@Lucky: Må finne en måte å få navn på ting inn i suggestion før den er adda til lista. 
                // Entityen er klar for å ta imot informasjon men denne metoden fungerer annerledes en enkelt-tilfeller
                // Må basically per response ta SuggestedUserId = AspNetUsers.Id og få empFname og empLname
                // Klokka er nøyaktig 5:43 og jeg sliter i hodet takk
                // Kan evt se på metoden nedenfor og se om du kan tenke deg til noe som ligner
            }
            connection.Close();

            var query = "";


            //Loop to get Suggested users name
            foreach (var suggestion in suggestions)
            {
                System.Diagnostics.Debug.WriteLine(suggestion.SuggestionTitle);
                connection.Open();
                query = ("SELECT empFname, empLname FROM AspNetUsers WHERE Id = '" + suggestion.SuggestedUserId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    suggestion.SuggestedFirstname = reader.GetString(0);
                    suggestion.SuggestedLastname = reader.GetString(1);
                }
                connection.Close();
            }

            //Loop to get Responsible users name
            foreach (var suggestion in suggestions)
            {
                System.Diagnostics.Debug.WriteLine(suggestion.SuggestionTitle);
                connection.Open();
                query = ("SELECT empFname, empLname FROM AspNetUsers WHERE Id = '" + suggestion.ResponsibleUserId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    suggestion.ResponsibleFirstname = reader.GetString(0);
                    suggestion.ResponsibleLastname = reader.GetString(1);
                }
                connection.Close();
            }

            //Loop to get Team name
            foreach (var suggestion in suggestions)
            {
                System.Diagnostics.Debug.WriteLine(suggestion.SuggestionTitle);
                connection.Open();
                query = ("SELECT TeamName FROM Team WHERE TeamId = '" + suggestion.TeamId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    suggestion.TeamName = reader.GetString(0);
                }
                connection.Close();
            }

            //Loop to get Status title
            foreach (var suggestion in suggestions)
            {
                System.Diagnostics.Debug.WriteLine(suggestion.SuggestionTitle);
                connection.Open();
                query = ("SELECT StatusTitle FROM Status WHERE StatusId = '" + suggestion.StatusId + "';");
                reader = ReadData(query, connection);
                while (reader.Read())
                {
                    suggestion.StatusName = reader.GetString(0);
                }
                connection.Close();
            }

            return suggestions;
        }
        public SuggestionModel GetSuggestionById(int id)
        {
            //This method is probably highly ineffective but it works for now
            //First gets the suggestion
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, SuggestedUserId, ResponsibleUserId, TeamId, StatusId FROM suggestion WHERE SuggestionId = " + id + ";");
            var reader = ReadData(query, connection);
            var suggestion = new SuggestionModel();
            while (reader.Read())
            {
                suggestion.SuggestionId = reader.GetInt32("TeamId");
                suggestion.SuggestionTitle = reader.GetString(1);
                suggestion.SuggestionDescription = reader.GetString(2);
                suggestion.SuggestionDeadline = reader.GetDateTime(3);
                if (!reader.IsDBNull(4))
                    suggestion.SuggestionEndDate = reader.GetDateTime(4);
                else suggestion.SuggestionEndDate = DateTime.MinValue;
                suggestion.SuggestedUserId= reader.GetString(5);
                suggestion.ResponsibleUserId = reader.GetString(6);
                suggestion.TeamId = reader.GetInt32(7);
                suggestion.StatusId= reader.GetInt32(8);
            }
            connection.Close();

            //Takes suggested User ID and finds a name
            connection.Open();
            query = ("SELECT `empFname`, `empLname` FROM `AspNetUsers` WHERE Id = '" + suggestion.SuggestedUserId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                suggestion.SuggestedFirstname = reader.GetString(0);
                suggestion.SuggestedLastname = reader.GetString(1);
            }
            connection.Close();

            //Takes Responsible User ID and finds a name
            connection.Open();
            query = ("SELECT `empFname`, `empLname` FROM `AspNetUsers` WHERE Id = '" + suggestion.ResponsibleUserId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                suggestion.ResponsibleFirstname = reader.GetString(0);
                suggestion.ResponsibleLastname = reader.GetString(1);
            }
            connection.Close();

            //Takes team ID and finds the team name
            connection.Open();
            query = ("SELECT `TeamName` FROM `Team` WHERE TeamId = '" + suggestion.TeamId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                suggestion.TeamName = reader.GetString(0);
            }
            connection.Close();

            //Takes status ID and finds the status name
            connection.Open();
            query = ("SELECT `StatusTitle` FROM `Status` WHERE StatusId = '" + suggestion.StatusId + "';");
            reader = ReadData(query, connection);
            while (reader.Read())
            {
                suggestion.StatusName = reader.GetString(0);
            }
            connection.Close();

            return suggestion;
        }

        public int CreateSuggestion(string sugTitle, string sugDesc, DateTime sugDea, string sugUse, string resUse, int teamId)
        {
            var dateValue = sugDea; //Takes supplied DateTime
            string formatDateForMySql = dateValue.ToString("yyyy-MM-dd HH:mm:ss"); //Then converts it into a format MySQL understands (or we'd all be stuck in year 0)

            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            var newSuggestionId = (FindHighestId() + 1);
            connection.Open();
            var query = ("INSERT INTO `suggestion` (`SuggestionId`, `SuggestionTitle`, `SuggestionDescription`, `SuggestionDeadline`, `SuggestionEnddate`, `SuggestedUserId`, `ResponsibleUserId`, `TeamId`, `StatusId`) VALUES ('" + newSuggestionId + "', '" + sugTitle + "', '" + sugDesc + "', '" + formatDateForMySql + "', NULL, '" + sugUse + "', '" + resUse + "', '" + teamId + "', '0');");
            System.Diagnostics.Debug.WriteLine(query);
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
            return newSuggestionId; //Returns ID for added suggestion so Controller can redirect to the page for it
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

        public void DeleteSuggestion(int suggestionId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("DELETE FROM suggestion WHERE `suggestion`.`SuggestionId` = " + suggestionId + ";");
            var reader = ReadData(query, connection);
            reader.Read();
            connection.Close();
        }

        public int FindHighestId()
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("SELECT SuggestionId FROM `suggestion` WHERE `SuggestionId` = (SELECT MAX(SuggestionId) FROM suggestion);");
            var reader = ReadData(query, connection);
            int highestId = 0;
            while (reader.Read())
            {
                highestId = reader.GetInt32("SuggestionId");
            }
            connection.Close();
            return highestId;
        }

        public void PopulateStatusInDB()
        {
            //Highly ineffective way (probably) of making sure all statuses are present
            //Basically tries to insert the statuses every time SuggestionsController is constructed
            //Brain is too fried to do it any other way
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            connection.Open();
            var query = ("INSERT INTO `status` (StatusId, StatusTitle) VALUES (0, 'Under vurdering') ON DUPLICATE KEY UPDATE StatusId = 0;INSERT INTO `status` (StatusId, StatusTitle) VALUES (1, 'Godtatt') ON DUPLICATE KEY UPDATE StatusId = 1;INSERT INTO `status` (StatusId, StatusTitle) VALUES (2, 'Avslått') ON DUPLICATE KEY UPDATE StatusId = 2;INSERT INTO `status` (StatusId, StatusTitle) VALUES (3, 'Pågår') ON DUPLICATE KEY UPDATE StatusId = 3;INSERT INTO `status` (StatusId, StatusTitle) VALUES (4, 'På pause') ON DUPLICATE KEY UPDATE StatusId = 4;INSERT INTO `status` (StatusId, StatusTitle) VALUES (5, 'Ferdig') ON DUPLICATE KEY UPDATE StatusId = 5;"); //Om du er like langt til høyre som slutten på denne stringen så har du gjort noe feil i livet
            var reader = ReadData(query, connection);
            connection.Close();
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
