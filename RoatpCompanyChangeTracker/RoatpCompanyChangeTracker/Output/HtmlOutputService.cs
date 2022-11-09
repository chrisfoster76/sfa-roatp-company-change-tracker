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
            writer.Write(@"<link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/css/bootstrap.min.css"" />");
            writer.WriteFullBeginTag("head");
            writer.WriteEndTag("head");

            writer.WriteFullBeginTag("body");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "container-fluid");
            writer.Write(HtmlTextWriter.SelfClosingTagEnd);


            writer.WriteFullBeginTag("title");
            writer.Write("RoATP Provider Company Change Tracker");
            writer.WriteEndTag("title");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "layout");
            writer.Write(">");

            writer.WriteBeginTag("div");
            writer.WriteAttribute("class", "header");
            writer.Write(">");
            writer.WriteFullBeginTag("h1");
            writer.Write("RoATP Provider Company Change Tracker");
            writer.WriteEndTag("h1");
            writer.WriteEndTag("div");
            writer.Write("<br/>");


            var outputGroup = output.GroupBy(x => x.EventDate);

            foreach (var groupItem in outputGroup.OrderByDescending(x => x.Key))
            {
                writer.WriteBeginTag("div");
                writer.WriteAttribute("style", "background-color: #fff; border-bottom: 2px solid #333; margin-bottom: 1em;");
                writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                writer.WriteFullBeginTag("h2");
                writer.Write(groupItem.Key.ToLongDateString());
                writer.WriteEndTag("h2");
                writer.WriteEndTag("div"); //callout
               

                var providerGroup = output.Where(x => x.EventDate == groupItem.Key).GroupBy(x => x.Ukprn);

                foreach (var provider in providerGroup)
                {
                    var roatpProvider = providers.First(x => x.Ukprn == provider.Key);

                    var items = output.Where(x => x.Ukprn == provider.Key && x.EventDate == groupItem.Key);

                    var providerCompanyGroup = items.GroupBy(x => x.CompanyName);

                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", "card");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                    writer.WriteBeginTag("h4");
                    writer.WriteAttribute("class", "card-header");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                    writer.Write($"{roatpProvider.Name}");
                    writer.WriteBeginTag("small");
                    writer.WriteAttribute("class", "text-muted");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                    writer.Write(HtmlTextWriter.SpaceChar);
                    writer.Write($"UKPRN {provider.Key}");
                    writer.WriteEndTag("small");
                    writer.WriteEndTag("h4");

                    writer.WriteBeginTag("div");
                    writer.WriteAttribute("class", "card-body");
                    writer.Write(HtmlTextWriter.SelfClosingTagEnd);


                    foreach (var providerCompany in providerCompanyGroup)
                    {
                        var providerCompanyItems = items.Where(x => x.CompanyName == providerCompany.Key);

                        var firstProviderCompanyItem = providerCompanyItems.First();

                        writer.WriteFullBeginTag("h5");
                        writer.WriteBeginTag("a");
                        writer.WriteAttribute("style", "text-decoration: none;");
                        writer.WriteAttribute("class", "text-reset");
                        writer.WriteAttribute("href", $"https://find-and-update.company-information.service.gov.uk/company/{roatpProvider.CompanyNumber}");
                        writer.WriteAttribute("target", "_blank");
                        writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                        writer.Write($"{firstProviderCompanyItem.CompanyName}");
                        writer.WriteEndTag("a");
                        writer.WriteEndTag("h5");

                        if (firstProviderCompanyItem.IsParentCompany)
                        {
                            writer.WriteBeginTag("div");
                            writer.WriteAttribute("class", "fw-light");
                            writer.WriteAttribute("style", "padding-bottom: 0.5em");
                            writer.Write(HtmlTextWriter.SelfClosingTagEnd);

                            var pathCount = 0;
                            foreach (var pathItem in firstProviderCompanyItem.Path)
                            {
                                writer.WriteFullBeginTag("span");
                                if(pathCount!=0) {writer.Write(" > ");}

                                writer.WriteBeginTag("a");
                                writer.WriteAttribute("href", $"https://find-and-update.company-information.service.gov.uk/company/{pathItem.CompanyNumber}");
                                writer.WriteAttribute("target", "_blank");
                                writer.WriteAttribute("style", "text-decoration: none;");
                                writer.WriteAttribute("class", "text-reset");
                                writer.WriteAttribute("class", "fs-5");
                                writer.Write(HtmlTextWriter.SelfClosingTagEnd);
                                writer.Write(pathItem.CompanyName);
                                writer.WriteEndTag("a");

                                writer.WriteEndTag("span");
                                pathCount++;
                            }

                            
                            writer.WriteEndTag("div");
                        }



                        writer.WriteFullBeginTag("ul");

                        foreach (var item in providerCompanyItems)
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

                    writer.WriteEndTag("div"); //card body
                    writer.WriteEndTag("div"); //card
                    writer.WriteFullBeginTag("br");
                    writer.WriteEndTag("br");

                }
            }


            writer.WriteEndTag("div");
            writer.WriteEndTag("div"); //fluid container
            writer.WriteEndTag("body");

            writer.WriteEndTag("html");

            

            File.WriteAllText(@"c:\temp\roatp-company-change-tracker.html", stringWriter.ToString());
        }
    }
}
