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
                var users = connection.Query<SuggestionEntity>("Select SuggestionId, SuggestionTitle, SuggestionDescription, SuggestionDeadline, SuggestionEnddate, SuggestedUserId, ResponsibleUserId, TeamId, StatusId  from Suggestion;");
                return users.ToList();
            }
        }

    }
}
