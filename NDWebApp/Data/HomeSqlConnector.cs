using MySqlConnector;
using System.Data;
using NDWebApp.Entities;
using NDWebApp.Models;
using System.Data.Common;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.Metrics;

namespace NDWebApp.Data
{
    public class HomeSqlConnector : IHomeSqlConnector
    {
        private readonly IConfiguration config;

        public HomeSqlConnector(IConfiguration config)
        {
            this.config = config;
        }

        public StatisticsModel GetStatistics(string userId)
        {
            using var connection = new MySqlConnection(config.GetConnectionString("NDWebAppContextConnection"));
            StatisticsModel statistics = new StatisticsModel();

            connection.Open();
            var reader = ReadData("SELECT StatusId FROM Suggestion;", connection);
            while (reader.Read())
            { 
                if (reader.GetInt32(0) == 0)
                {
                    statistics.SuggestionsAllUnderVurdering = statistics.SuggestionsAllUnderVurdering + 1;
                }
                else if (reader.GetInt32(0) == 1)
                {
                    statistics.SuggestionsAllGodtatt = statistics.SuggestionsAllGodtatt + 1;
                }
                else if (reader.GetInt32(0) == 2)
                {
                    statistics.SuggestionsAllAvslatt = statistics.SuggestionsAllAvslatt + 1;
                }
                else if (reader.GetInt32(0) == 3)
                {
                    statistics.SuggestionsAllPagar = statistics.SuggestionsAllPagar + 1;
                }
                else if (reader.GetInt32(0) == 4)
                {
                    statistics.SuggestionsAllPaPause = statistics.SuggestionsAllPaPause + 1;
                }
                else if (reader.GetInt32(0) == 5)
                {
                    statistics.SuggestionsAllFerdig = statistics.SuggestionsAllFerdig + 1;
                }
            }

            statistics.SuggestionsAllCount = statistics.SuggestionsAllUnderVurdering + statistics.SuggestionsAllGodtatt + statistics.SuggestionsAllAvslatt + statistics.SuggestionsAllPagar + statistics.SuggestionsAllPaPause + statistics.SuggestionsAllFerdig;
            connection.Close();

            connection.Open();
            reader = ReadData("SELECT StatusId FROM Suggestion WHERE SuggestedUserId = '"+ userId + "';", connection);
            while (reader.Read())
            {
                if (reader.GetInt32(0) == 0)
                {
                    statistics.SuggestionsUnderVurdering = statistics.SuggestionsUnderVurdering + 1;
                }
                else if (reader.GetInt32(0) == 1)
                {
                    statistics.SuggestionsGodtatt = statistics.SuggestionsGodtatt + 1;
                }
                else if (reader.GetInt32(0) == 2)
                {
                    statistics.SuggestionsAvslatt = statistics.SuggestionsAvslatt + 1;
                }
                else if (reader.GetInt32(0) == 3)
                {
                    statistics.SuggestionsPagar = statistics.SuggestionsPagar + 1;
                }
                else if (reader.GetInt32(0) == 4)
                {
                    statistics.SuggestionsPaPause = statistics.SuggestionsPaPause + 1;
                }
                else if (reader.GetInt32(0) == 5)
                {
                    statistics.SuggestionsFerdig = statistics.SuggestionsFerdig + 1;
                }
            }
            statistics.SuggestionsCount = statistics.SuggestionsUnderVurdering + statistics.SuggestionsGodtatt + statistics.SuggestionsAvslatt + statistics.SuggestionsPagar + statistics.SuggestionsPaPause + statistics.SuggestionsFerdig;
            connection.Close();

            return statistics;
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
