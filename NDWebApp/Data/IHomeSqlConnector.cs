using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface IHomeSqlConnector
    {
        StatisticsModel GetStatistics(string userId);
    }
}
