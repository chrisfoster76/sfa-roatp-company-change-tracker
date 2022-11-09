using Dapper;
using Microsoft.Data.SqlClient;
using RoatpCompanyChangeTracker.Config;
using RoatpCompanyChangeTracker.Models.Data.Baseline;

namespace RoatpCompanyChangeTracker.Storage.Baseline
{
    internal class BaseLineStorageService
    {
        private readonly SqlConnection _sqlConnection;

        private readonly List<CompanyData> _companyCache;

        public BaseLineStorageService()
        {
            var configService = new ConfigurationService();
            var config = configService.GetAppConfig();

            _sqlConnection = new SqlConnection(config.DatabaseConnectionString);

            _companyCache = new List<CompanyData>();
        }

        public async Task<CompanyData> GetCompany(string companyNumber)
        {
            var company = _companyCache.FirstOrDefault(x => x.CompanyNumber == companyNumber);

            if (company != null)
            {
                return company;
            }

            var sql = $"select [CompanyNumber], [RootCompanyNumber], [ParentCompanyNumber], [CompanyName], [PscData], [OfficersData], [ProfileData], [FilingHistoryData] from [ProviderCompany] where CompanyNumber = '{companyNumber}'";

            try
            {
                var result = await _sqlConnection.QueryFirstOrDefaultAsync<CompanyData>(sql, new {companyNumber=companyNumber});
                
                if(result!= null) { _companyCache.Add(result); }
                
                
                return result;
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }
        }

        public async Task<List<CompanyData>> GetCompanies()
        {
            var sql = $"select [CompanyNumber], [RootCompanyNumber], [ParentCompanyNumber], [CompanyName], [PscData], [OfficersData], [ProfileData], [FilingHistoryData] from [ProviderCompany]";

            try
            {
                var result = await _sqlConnection.QueryAsync<CompanyData>(sql);
                return result.ToList();
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error {ex.Message}");
                throw;
            }
        }
    }
}
