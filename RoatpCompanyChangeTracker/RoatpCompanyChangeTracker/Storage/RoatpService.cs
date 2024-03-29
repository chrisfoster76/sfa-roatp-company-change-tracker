﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoatpCompanyChangeTracker.Models.Data;

namespace RoatpCompanyChangeTracker.Storage
{
    internal class RoatpService
    {
        internal List<RoatpProvider> GetProviders()
        {
            Console.WriteLine("Loading providers...");

            var providerdata = File.ReadAllLines(@"localdata\ProviderCompanies.txt");
            Console.WriteLine($"{providerdata.Count()} providers loaded");

            var result = new List<RoatpProvider>();

            foreach (var line in providerdata)
            {
                var splitData = line.Split(',');

                result.Add(new RoatpProvider
                {
                    Ukprn = splitData[0],
                    CompanyNumber = splitData[1],
                    Name = splitData[2]
                });

            }

            return result;

        }
    }
}
