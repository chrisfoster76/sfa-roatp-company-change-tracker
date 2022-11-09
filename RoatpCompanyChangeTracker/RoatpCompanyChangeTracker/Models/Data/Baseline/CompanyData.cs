using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoatpCompanyChangeTracker.Models.Data.Baseline
{
    internal class CompanyData
    {
        public string CompanyNumber { get; set; }
        public string? RootCompanyNumber { get; set; }
        public string? ParentCompanyNumber { get; set; }
        public string Ukprn { get; set; }
        public string CompanyName { get; set; }
        public string ProfileData { get; set; }
        public string OfficersData { get; set; }
        public string PscData { get; set; }
        public string FilingHistoryData { get; set; }

        public CompanyProfile CompanyProfile => JsonSerializer.Deserialize<CompanyProfile>(ProfileData);

        public PscDataSet Pscs => JsonSerializer.Deserialize<PscDataSet>(PscData);
        
        public OfficersDataSet Officers
        {
            get { return OfficersData == null ? null : JsonSerializer.Deserialize<OfficersDataSet>(OfficersData); }
        }

        public FilingHistoryDataSet FilingHistoryDataSet => JsonSerializer.Deserialize<FilingHistoryDataSet>(FilingHistoryData);
    }


    internal class CompanyProfile
    {
        [JsonPropertyName("company_status")]
        public string Status { get; set; }
    }
}
