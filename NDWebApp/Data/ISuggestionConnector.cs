using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface ISuggestionConnector
    {
        IEnumerable<SuggestionEntity> GetSuggestions();

        SuggestionModel GetSuggestionById(int id);

        void PopulateStatusInDB();

        int CreateSuggestion(string sugTitle, string sugDesc, DateTime sugDea, string sugUse, string resUse, int teamId);

        void DeleteSuggestion(int suggestionId);

        IEnumerable<UserEntity> GetUsers();

        IEnumerable<TeamEntity> GetTeams();

        IDbConnection GetDbConnection();
    }
}
