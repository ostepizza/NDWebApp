using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using NDWebApp.Entities;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NDWebApp.Data
{
    public class SuggestionRepository : ISuggestionRepository
    {
        private readonly ISqlConnection sqlConnector;

        public SuggestionRepository(ISqlConnection sqlConnector)
        {
            this.sqlConnector = sqlConnector;
        }
       
        public List<SuggestionEntity> GetSuggestions()
        {
            using (var connection = sqlConnector.GetDbConnection() as MySqlConnection)
            {
                var users = connection.Query<SuggestionEntity>("SELECT SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, SuggestedUserId, ResponsibleUserId, TeamId, StatusId  FROM Suggestion;");
                return users.ToList();
            }
        }

        public void Delete(int id)
        {
            var user = GetSuggestionById(id);
            if (user == null)
            {
                return;
            }
            using (var connection = sqlConnector.GetDbConnection() as MySqlConnection)
            {
                connection.QueryFirstOrDefault("DELETE FROM suggestion WHERE SuggestionId = @idParameter",
                new { idParameter = id });
            }
        }
        
        public SuggestionEntity GetSuggestionById(int id)
        {
            using (var connection = sqlConnector.GetDbConnection() as MySqlConnection)
            {
                return connection.QueryFirstOrDefault("SELECT SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, SuggestedUserId, ResponsibleUserId, TeamId, StatusId  FROM Suggestion WHERE SuggestionId = @idParameter",
                new { idParameter = id});
            }
        }
    }
}
