using System.Globalization;

namespace RoatpCompanyChangeTracker.Models.Data.Events
{
    internal class StreamingApiEvent
    {
        public long Timepoint { get; set; }
        public string ResourceUri { get; set; }

        public string Type { get; set; }

        public string Data { get; set; }

        public string ProviderCompanyNumber
        {
            get
            {
                return ResourceUri.Substring(9, 8);
            }
        }
    }
}
