namespace RoatpCompanyChangeTracker.Models
{
    internal class OutputItem
    {
        public DateTime EventDate { get; set; }
        public DateTime ActionDate { get; set; }
        public string Ukprn { get; set; }
        public bool IsParentCompany { get; set; }
        public string CompanyName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public List<PathItem> Path { get; set; }
        public string CompanyNumber { get; set; }
    }

    internal class PathItem
    {
        public string CompanyName { get; set; }
        public string CompanyNumber { get; set; }

    }
}
