using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data;

namespace NDWebApp.Data
{
    public interface IRepairsSqlConnector
    {
        IEnumerable<RepairsEntity> GetRepairs();

        RepairsModel GetRepairById(int id);

        IDbConnection GetDbConnection();
    }
}
