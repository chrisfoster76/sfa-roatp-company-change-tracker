// See https://aka.ms/new-console-template for more information

using RoatpCompanyChangeTracker.Models;
using RoatpCompanyChangeTracker.Output;
using RoatpCompanyChangeTracker.Storage;
using RoatpCompanyChangeTracker.Storage.Baseline;
using RoatpCompanyChangeTracker.Storage.Events;

var filingHistoryEnumerationService = new FilingHistoryEnumerationService();



var providerService = new RoatpService();
var providers = providerService.GetProviders();

var baselineStorageService = new BaseLineStorageService();

var count = 0;


long pscTimepoint = 0;
long officersTimepoint = 0;
long profileTimepoint = 0;
var eventStore = new EventStorageService();


//while (true)
//{
//    var nextEvent = await eventStore.GetNextEvent<GenericEvent<ProfileData>>("CompanyProfile", profileTimepoint);

//    if (nextEvent == null)
//    {
//        break;
//    }

//    profileTimepoint = nextEvent.Timepoint;

//    if (!providers.Any(x => x.CompanyNumber == nextEvent.ProviderCompanyNumber))
//    {
//        continue;
//    }

//    var company = await baselineStorageService.GetCompany(nextEvent.ProviderCompanyNumber);

//    if (company == null) continue;

//    if (company.CompanyProfile.Status != nextEvent.GenericData.Status)
//    {
//        Console.WriteLine("Status changed");
//    }
//}


//Console.WriteLine("--------------");


//while (true)
//{
//    var nextEvent = await eventStore.GetNextEvent<GenericEvent<PscData>>("PersonsWithSignificantControl", pscTimepoint);

//    if (nextEvent == null)
//    {
//        break;
//    }

//    pscTimepoint = nextEvent.Timepoint;

//    if (!providers.Any(x => x.CompanyNumber == nextEvent.ProviderCompanyNumber))
//    {
//        continue;
//    }

//    var company = await baselineStorageService.GetCompany(nextEvent.ProviderCompanyNumber);

//    if (company == null) continue;

//    if (company.CompanyName.Contains("ahead"))
//    {
//        Console.WriteLine("*****");
//    }

//    if (nextEvent.Type == "changed")
//    {
//        if (!company.Pscs.Items.Any(x => x.Links.Self == nextEvent.ResourceUri))
//        {
//            company.Pscs.Items.Add(new PscDataSet.Item { Links = new PscDataSet.Links { Self = nextEvent.ResourceUri } });
//            count++;
//        }
//        else
//        {
//            if (!string.IsNullOrWhiteSpace(nextEvent.GenericData.EndDate))
//            {
//                var officer = company.Pscs.Items.First(x => x.Links.Self == nextEvent.ResourceUri);
//                if (nextEvent.GenericData.EndDate != officer.CeasedOn)
//                {
//                    Console.WriteLine($"Ceased PSC {nextEvent?.GenericData.Name} for Company {company.CompanyNumber} {company.CompanyName} timepoint {nextEvent.Timepoint}");
//                    count++;
//                }
//            }
//        }
//    }
//    else
//    {
//        if (company.Pscs.Items.Any(x => x.Links.Self == nextEvent.ResourceUri))
//        {
//            company.Pscs.Items.RemoveAll(x => x.Links.Self == nextEvent.ResourceUri);
//            Console.WriteLine($"Deleted PSC for Company {company.CompanyNumber}");
//            count++;
//        }
//    }
//}



//Console.WriteLine("--------------");


//while (true)
//{
//    var nextEvent = await eventStore.GetNextEvent<GenericEvent<OfficerData>>("Officers", officersTimepoint);

//    if (nextEvent == null)
//    {
//        break;
//    }

//    officersTimepoint = nextEvent.Timepoint;

//    if (!providers.Any(x => x.CompanyNumber == nextEvent.ProviderCompanyNumber))
//    {
//        continue;
//    }

//    var company = await baselineStorageService.GetCompany(nextEvent.ProviderCompanyNumber);

//    if (company == null) continue;
//    if (company.Officers == null) continue;
    

//    if (nextEvent.Type == "changed")
//    {
//        if (!company.Officers.Items.Any(x => x.Links.Self == nextEvent.ResourceUri))
//        {
//            company.Officers.Items.Add(new OfficersDataSet.Item { Name = nextEvent.GenericData.Name, ResignedOn = nextEvent.GenericData.EndDate, Links = new OfficersDataSet.Links { Self = nextEvent.ResourceUri } });
//            Console.WriteLine($"New Officer {nextEvent.GenericData.Name} for Company {company.CompanyNumber} {company.CompanyName} timepoint {nextEvent.Timepoint}");
//            count++;
//        }
//        else
//        {
           
//            if (!string.IsNullOrWhiteSpace(nextEvent.GenericData.EndDate))
//            {
//                var officer = company.Officers.Items.First(x => x.Links.Self == nextEvent.ResourceUri);
//                if (nextEvent.GenericData.EndDate != officer.ResignedOn)
//                {
//                    Console.WriteLine($"Termination of appointment: {nextEvent?.GenericData?.Name} for Company {company.CompanyNumber} timepoint {nextEvent.Timepoint}");
//                    count++;
//                }
//            }
//        }
//    }
//    else
//    {
//        if (company.Officers.Items.Any(x => x.Links.Self == nextEvent.ResourceUri))
//        {
//            company.Officers.Items.RemoveAll(x => x.Links.Self == nextEvent.ResourceUri);
//            Console.WriteLine($"Deleted Officer for Company {company.CompanyNumber}");
//            count++;
//        }
//    }
//}

//Console.WriteLine("------------------------------");

var output = new List<OutputItem>();

foreach (var provider in providers)
{
    var hasShowCompanyName = false;
    var company = await baselineStorageService.GetCompany(provider.CompanyNumber);

    if (company == null)
    {
        continue;
    }

    if (company.FilingHistoryDataSet.items != null)
    {
        foreach (var item in company.FilingHistoryDataSet.items)
        {
            if (item.ActionDate > DateTime.Now.Date.AddDays(-365))// new DateTime(2021,12,31)) //
            {
                if (item.IsOfInterest)
                {
                    var outputItem = new OutputItem
                    {
                        ActionDate = item.ActionDate.Value,
                        CompanyName = company.CompanyName,
                        Ukprn = provider.Ukprn,
                        Category = item.category,
                        Description = filingHistoryEnumerationService.GetDescription(item),
                        //Description = item.description,
                        IndividualName = item.category == "officers" ? item.description_values.officer_name : item.description_values.psc_name

                    };
                    output.Add(outputItem);
                    

                    count++;
                    //if (!hasShowCompanyName)
                    //{
                    //    Console.WriteLine("");
                    //    Console.WriteLine($"{company.CompanyName} - ukprn {provider.Ukprn} - company number {company.CompanyNumber}");
                    //}
                    //hasShowCompanyName = true;

                    //Console.WriteLine($"{item.ActionDate.Value.Date.ToString("yyyy-MM-dd")} - {item.category} - {item.description}");

                    //if (item.category == "officers")
                    //{
                    //    Console.WriteLine($" -> {item.description_values.officer_name}");
                    //} else if (item.category == "persons-with-significant-control")
                    //{
                    //    Console.WriteLine($" -> {item.description_values.psc_name}");
                    //}
                    //else
                    //{
                    //    Console.WriteLine(item.description);
                    //}
                }
            }


        }
    }
}


var outputService = new HtmlOutputService();
outputService.GenerateOutput(output, providers);



Console.WriteLine($"Finished - {count}");
Console.ReadKey();