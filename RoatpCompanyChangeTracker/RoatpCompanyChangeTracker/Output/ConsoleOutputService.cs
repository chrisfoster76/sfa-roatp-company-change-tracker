using RoatpCompanyChangeTracker.Models;
using RoatpCompanyChangeTracker.Models.Data;

namespace RoatpCompanyChangeTracker.Output
{
    internal class ConsoleOutputService : IOutputService
    {
        public void GenerateOutput(List<OutputItem> output, List<RoatpProvider> providers)
        {
            var outputGroup = output.GroupBy(x => x.ActionDate);

            foreach (var groupItem in outputGroup.OrderByDescending(x => x.Key))
            {
                Console.WriteLine(groupItem.Key.ToString("yyyy-MM-dd"));
                Console.WriteLine();

                var providerGroup = output.Where(x => x.ActionDate == groupItem.Key).GroupBy(x => x.Ukprn);

                foreach (var provider in providerGroup)
                {
                    var items = output.Where(x => x.Ukprn == provider.Key && x.ActionDate == groupItem.Key);

                    var roatpProvider = providers.First(x => x.Ukprn == provider.Key);

                    Console.WriteLine($"{roatpProvider.Name} - UKPRN {provider.Key}");
                    Console.WriteLine($"Companies House Number: {roatpProvider.CompanyNumber}");
                    foreach (var item in items)
                    {
                        Console.WriteLine($"{item.Description}");
                    }
                    Console.WriteLine();
                }

                Console.WriteLine();
            }

        }
    }
}
