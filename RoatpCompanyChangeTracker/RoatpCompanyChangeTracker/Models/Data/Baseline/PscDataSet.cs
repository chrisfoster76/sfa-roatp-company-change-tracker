using System.Text.Json.Serialization;

namespace RoatpCompanyChangeTracker.Models.Data.Baseline
{
    internal class PscDataSet
    {
        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        public class Item
        {
            [JsonPropertyName("links")]
            public Links Links { get; set; }
            
            [JsonPropertyName(("ceased_on"))]
            public string CeasedOn { get; set; }
        }

        public class Links
        {
            [JsonPropertyName("self")]
            public string Self { get; set; }
        }
    }

    internal class OfficersDataSet
    {
        [JsonPropertyName("items")]
        public List<OfficersDataSet.Item> Items { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        public class Item
        {
            [JsonPropertyName("links")]
            public OfficersDataSet.Links Links { get; set; }

            public string Name { get; set; }

            [JsonPropertyName(("resigned_on"))]
            public string ResignedOn { get; set; }
        }

        public class Links
        {
            [JsonPropertyName("self")]
            public string Self { get; set; }
        }

    }

}
