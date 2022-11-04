using Microsoft.Data.SqlClient;
using RoatpCompanyChangeTracker.Config;
using Dapper;
using RoatpCompanyChangeTracker.Models.Data.Events;

namespace RoatpCompanyChangeTracker.Storage.Events
{
    internal class EventStorageService
    {
        private readonly SqlConnection _sqlConnection;

        public EventStorageService()
        {
            var configService = new ConfigurationService();
            var config = configService.GetAppConfig();

            _sqlConnection = new SqlConnection(config.EventDatabaseConnectionString);
        }

        public async Task<T> GetNextEvent<T>(string eventType, long timepoint)
        {
            var sql = $"select top 1 [Timepoint], [ResourceUri], [Type], [Data] from [{eventType}] where Timepoint > {timepoint} and IsRoatpProvider = 1";

            try
            {
                var result = await _sqlConnection.QueryFirstOrDefaultAsync<T>(sql);
                return result;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }
        }
    }
}
