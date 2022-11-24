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

                Console.WriteLine(reader.GetString(1));
                suggestions.Add(suggestion);
            }
            connection.Close();
            return suggestions;
        }
        public SuggestionModel GetSuggestionById(int id)
        {
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
            return suggestion;
        }

        public void PopulateStatusInDB()
        {
            //Highly ineffective way of making sure all statuses are present
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
