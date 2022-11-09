using RoatpCompanyChangeTracker.Models.Data.Baseline;

namespace RoatpCompanyChangeTracker.Storage
{
    internal class FilingHistoryEnumerationService
    {
        private readonly Dictionary<string, string> _data;

        public FilingHistoryEnumerationService()
        {
            _data = new Dictionary<string, string>();

            var data = File.ReadAllLines(@"localdata\FilingHistoryEnumerations.txt");

            foreach (var line in data)
            {
                var splitLine = line.Split(':');
                _data.Add(splitLine[0].Replace("\'", "").Trim(), splitLine[1].Replace("\"", "").Trim());
            }
        }

        public Dictionary<string, string> Data => _data;


        public string GetDescription(Item item)
        {
            if (!_data.ContainsKey(item.description))
            {
                return "";
            }

            if (item.description_values == null) return item.description;

            var description = _data[item.description];

            description = description.Replace("{officer_name}", item.description_values.officer_name);
            description = description.Replace("{termination_date}", item.description_values.termination_date);
            description = description.Replace("{appointment_date}", item.description_values.appointment_date);
            description = description.Replace("{psc_name}", item.description_values.psc_name);
            description = description.Replace("{notification_date}", item.action_date);
            description = description.Replace("{cessation_date}", item.description_values.termination_date);
            description = description.Replace("{change_date}", item.description_values.change_date);


            return description;
        }

    }
}
