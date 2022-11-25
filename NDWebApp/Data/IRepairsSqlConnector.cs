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

        int CreateRepair(string repTitle, string repDesc, DateTime repDea, string repUse, int teamId);

        void DeleteRepair(int repairId);

        void PopulateStatusInDB();
    }
}
