using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoatpCompanyChangeTracker.Models.Data.Events
{
    internal class GenericEvent<T> : StreamingApiEvent
    {
        public T GenericData => JsonSerializer.Deserialize<T>(Data);
    }

    internal class OfficerData : BaseData
    {
        [JsonPropertyName("resigned_on")]
        public override string EndDate { get; set; }
    }

    internal class PscData : BaseData
    {
        [JsonPropertyName("ceased_on")]
        public override string EndDate { get; set; }
    }

    internal abstract class BaseData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        public abstract string EndDate { get; set; }
    }

    internal class ProfileData
    {
        [JsonPropertyName("company_status")]
        public string Status { get; set; }
    }

}
