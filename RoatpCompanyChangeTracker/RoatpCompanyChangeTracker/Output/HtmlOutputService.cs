using System.Text.RegularExpressions;
using RoatpCompanyChangeTracker.Models;
using RoatpCompanyChangeTracker.Models.Data;
using System.Web.UI;

namespace RoatpCompanyChangeTracker.Output
{
    internal class HtmlOutputService : IOutputService
    {
        public void GenerateOutput(List<OutputItem> output, List<RoatpProvider> providers)
        {
            // Initialize StringWriter instance.
            var stringWriter = new StringWriter();

            // Put HtmlTextWriter in using block because it needs to call Dispose.
            using var writer = new HtmlTextWriter(stringWriter);

            writer.WriteFullBeginTag("html");
            writer.Write(@"<link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/purecss@3.0.0/build/pure-min.css"" integrity=""sha384-X38yfunGUhNzHpBaEBsWLO+A0HDYOQi8ufWDkZ0k9e0eXz/tH3II7uKZ9msv++Ls"" crossorigin=""anonymous"">");
            writer.WriteFullBeginTag("head");
            writer.WriteEndTag("head");


            writer.WriteFullBeginTag("body");
            writer.WriteFullBeginTag("title");
            writer.Write("RoATP Provider Company Change Tracker");
            writer.WriteEndTag("title");
            writer.WriteFullBeginTag("h1");
            writer.Write("RoATP Provider Company Change Tracker");
            writer.WriteEndTag("h1");
            


            var outputGroup = output.GroupBy(x => x.ActionDate);

            foreach (var groupItem in outputGroup.OrderByDescending(x => x.Key))
            {
                writer.WriteFullBeginTag("h2");
                //writer.Write(groupItem.Key.ToString("yyyy-MM-dd"));
                writer.Write(groupItem.Key.ToLongDateString());
                writer.WriteEndTag("h2");

                var providerGroup = output.Where(x => x.ActionDate == groupItem.Key).GroupBy(x => x.Ukprn);

                foreach (var provider in providerGroup)
                {
                    var items = output.Where(x => x.Ukprn == provider.Key && x.ActionDate == groupItem.Key);

                    var roatpProvider = providers.First(x => x.Ukprn == provider.Key);

                    writer.WriteFullBeginTag("h3");
                    writer.Write($"{roatpProvider.Name} - UKPRN {provider.Key}");
                    writer.WriteEndTag("h3");

                    writer.WriteBeginTag("a");
                    writer.WriteAttribute("href", $"https://find-and-update.company-information.service.gov.uk/company/{roatpProvider.CompanyNumber}");
                    writer.WriteAttribute("target", "_blank");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.Write($"Companies House Number: {roatpProvider.CompanyNumber}");
                    writer.WriteEndTag("a");
                    

                    writer.WriteFullBeginTag("ul");
                    
                    foreach (var item in items)
                    {
                        writer.WriteFullBeginTag("li");

                        var description = item.Description;

                        if (description.Contains("**"))
                        {
                            var regex = new Regex(Regex.Escape("**"));
                            description = regex.Replace(description, "<strong>", 1);
                            description = regex.Replace(description, "</strong>", 1);
                        }


                        writer.Write($"{description}");
                        writer.WriteEndTag("li");
                    }

                    writer.WriteEndTag("ul");
                }

                writer.WriteFullBeginTag("hr");
                writer.WriteEndTag("hr");
            }

            writer.WriteEndTag("body");

            writer.WriteEndTag("html");

            

            File.WriteAllText(@"c:\temp\roatp-company-change-tracker.html", stringWriter.ToString());
        }
    }
}
