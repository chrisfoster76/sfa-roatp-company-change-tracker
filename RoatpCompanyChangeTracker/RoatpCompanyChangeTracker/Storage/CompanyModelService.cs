//using Microsoft.Data.SqlClient;
//using RoatpCompanyChangeTracker.Config;
//using RoatpCompanyChangeTracker.Models.Data.Baseline;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using RoatpCompanyChangeTracker.Models;
//using RoatpCompanyChangeTracker.Storage.Baseline;

//namespace RoatpCompanyChangeTracker.Storage
//{
//    internal class CompanyModelService
//    {
//        private BaseLineStorageService _baseLineStorageService;

//        public CompanyModelService()
//        {
//            _baseLineStorageService = new BaseLineStorageService();
//        }

//        public async Task<CompanyModel> GetCompany(string companyNumber)
//        {
//            var companyData = await _baseLineStorageService.GetCompany(companyNumber);


//            var result = new CompanyModel();

//            result.Officers = new List<OfficerModel>();

//            foreach (var officerData in companyData.Officers.Items)
//            {
//                result.Officers.Add(new OfficerModel
//                {
//                    Id = officerData.Links.Self,
//                    ResignedOn = officerData.
//                });
//            }


//            return result;
//        }
//    }
//}
