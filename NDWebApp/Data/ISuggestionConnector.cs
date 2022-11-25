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

        void UpdateSuggestion(int SuggestionId, string SuggestionTitle, string SuggestionDescription, DateTime SuggestionDeadline, DateTime SuggestionEnddate, string ResponsibleUserId, int TeamId, int StatusId);

        void DeleteSuggestion(int suggestionId);

        IEnumerable<UserEntity> GetUsers();

        IEnumerable<TeamEntity> GetTeams();

        IEnumerable<StatusEntity> GetStatusList();

        IDbConnection GetDbConnection();
    }
}
