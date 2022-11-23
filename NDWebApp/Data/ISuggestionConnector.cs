using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface ISuggestionConnector
    {
        IEnumerable<SuggestionEntity> GetSuggestions();

        SuggestionModel GetSuggestionById(int id);


        IDbConnection GetDbConnection();
    }
}
