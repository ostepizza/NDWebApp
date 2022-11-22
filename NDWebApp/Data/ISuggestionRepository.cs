using NDWebApp.Entities;

namespace NDWebApp.Data
{
    public interface ISuggestionRepository
    {
        List<SuggestionEntity> GetSuggestions();
    }
}
