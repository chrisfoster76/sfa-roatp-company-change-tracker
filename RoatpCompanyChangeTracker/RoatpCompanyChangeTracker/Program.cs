using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using RoatpCompanyChangeTracker.Models;
using RoatpCompanyChangeTracker.Output;
using RoatpCompanyChangeTracker.Storage;
using RoatpCompanyChangeTracker.Storage.Baseline;

var filingHistoryEnumerationService = new FilingHistoryEnumerationService();

var providerService = new RoatpService();
var providers = providerService.GetProviders();

var baselineStorageService = new BaseLineStorageService();

var count = 0;


var output = new List<OutputItem>();

var companies = await baselineStorageService.GetCompanies();

Console.WriteLine($"Loaded {companies.Count} companies");

foreach (var company in companies)
{
    
    

    var providerCompanyNumber = company.RootCompanyNumber ?? company.CompanyNumber;
   
    var provider = providers.SingleOrDefault(x => x.CompanyNumber == providerCompanyNumber);

    if (provider == null)
    {
        continue;
    }

    var path = new List<PathItem>();
    if (company.ParentCompanyNumber != null)
    {
        path.Add(new PathItem{CompanyNumber = company.CompanyNumber, CompanyName = company.CompanyName});

        var parentCompanyId = company.ParentCompanyNumber;

        while (parentCompanyId != null)
        {
            var parentCompany = await baselineStorageService.GetCompany(parentCompanyId);
            parentCompanyId = parentCompany.ParentCompanyNumber;
            path.Add(new PathItem { CompanyNumber = parentCompany.CompanyNumber, CompanyName = parentCompany.CompanyName });
        }
    }

    path.Reverse();


    if (company.FilingHistoryDataSet.items != null)
    {
        foreach (var item in company.FilingHistoryDataSet.items)
        {
            if (item.date > new DateTime(2022,05,09))
            {
                if (item.IsOfInterest)
                {
                    var outputItem = new OutputItem
                    {
                        Path = path,
                        EventDate = item.date,
                        IsParentCompany = !string.IsNullOrWhiteSpace(company.RootCompanyNumber),
                        ActionDate = item.ActionDate.HasValue ? item.ActionDate.Value : item.date,
                        CompanyName = company.CompanyName,
                        CompanyNumber = company.CompanyNumber,
                        Ukprn = provider.Ukprn,
                        Category = item.category,
                        Description = filingHistoryEnumerationService.GetDescription(item)
                    };
                    output.Add(outputItem);

                    count++;
                }
            }

        }
    }
}


//foreach (var provider in providers)
//{
//    var company = await baselineStorageService.GetCompany(provider.CompanyNumber);

//    if (company == null)
//    {
//        continue;
//    }

//    if (company.FilingHistoryDataSet.items != null)
//    {
//        foreach (var item in company.FilingHistoryDataSet.items)
//        {
//            if (item.ActionDate > DateTime.Now.Date.AddDays(-5))
//            {
//                if (item.IsOfInterest)
//                {
//                    var outputItem = new OutputItem
//                    {
//                        ActionDate = item.ActionDate.Value,
//                        CompanyName = company.CompanyName,
//                        Ukprn = provider.Ukprn,
//                        Category = item.category,
//                        Description = filingHistoryEnumerationService.GetDescription(item)

//                    };
//                    output.Add(outputItem);

//                    count++;
//                }
//            }

//        }
//    }
//}


var outputService = new HtmlOutputService();
    outputService.GenerateOutput(output, providers);



    Console.WriteLine($"Finished - {count}");
    Console.ReadKey();






