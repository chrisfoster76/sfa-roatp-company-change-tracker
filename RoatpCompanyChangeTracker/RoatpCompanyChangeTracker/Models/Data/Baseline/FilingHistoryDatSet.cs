using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace RoatpCompanyChangeTracker.Models.Data.Baseline
{
    public class AssociatedFiling
    {
        public object action_date { get; set; }
        public string category { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public DescriptionValues description_values { get; set; }
        public string type { get; set; }
        public Data data { get; set; }
    }

    public class Capital
    {
        public string currency { get; set; }
        public string figure { get; set; }
    }

    public class Data
    {
    }

    public class DescriptionValues
    {
        public string made_up_date { get; set; }
        public string description { get; set; }
        public string psc_name { get; set; }
        public string change_date { get; set; }
        public string charge_number { get; set; }
        public string officer_name { get; set; }
        public string appointment_date { get; set; }
        public string termination_date { get; set; }
        public string charge_creation_date { get; set; }
        public string new_date { get; set; }
        public string old_address { get; set; }
        public string res_type { get; set; }
        public List<Capital> capital { get; set; }
        public string date { get; set; }
        public string capital_figure { get; set; }
        public string capital_currency { get; set; }
    }

    public class Item
    {
        public DateTime? ActionDate
        {
            get
            {
                if (!string.IsNullOrEmpty(action_date))
                {
                    return DateTime.Parse(action_date);
                }

                return null;
            }
        }

        public bool IsOfInterest
        {
            get
            {
                if (category == "officers" )
                {
                    if (this.description_values.officer_name == "Gold Round Limited")
                    {

                                            Console.WriteLine(this.description);
                    }


                    if (description != "change-person-director-company-with-change-date" && description != "termination-director-company-with-name-termination-date" && description != "termination-secretary-company-with-name-termination-date"
                        && description != "change-person-director-company-with-change-date" && description != "change-person-secretary-company-with-change-date"
                        && description != "change-corporate-secretary-company-with-change-date" && description != "change-person-member-limited-liability-partnership-with-name-change-date"
                        && description != "change-corporate-director-company-with-change-date")
                    {


                        return true;
                    }
                }
                
                if (category == "persons-with-significant-control")
                {
                    //if (description == "cessation-of-a-person-with-significant-control" || description == "notification-of-a-person-with-significant-control")
                    if (description == "notification-of-a-person-with-significant-control")
                    {
                        return true;

                    }
                }

                return false;
            }
        }


        public string action_date { get; set; }
        public string category { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public DescriptionValues description_values { get; set; }
        public Links links { get; set; }
        public string type { get; set; }
        public int pages { get; set; }
        public string barcode { get; set; }
        public string transaction_id { get; set; }
        public bool? paper_filed { get; set; }
        public List<Resolution> resolutions { get; set; }
        public string subcategory { get; set; }
        public List<AssociatedFiling> associated_filings { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string document_metadata { get; set; }
    }

    public class Resolution
    {
        public string category { get; set; }
        public string description { get; set; }
        public string subcategory { get; set; }
        public string type { get; set; }
        public DescriptionValues description_values { get; set; }
    }

    public class FilingHistoryDataSet
    {
        public int items_per_page { get; set; }
        public List<Item> items { get; set; }
        public int total_count { get; set; }
        public string filing_history_status { get; set; }
        public int start_index { get; set; }
    }

}
