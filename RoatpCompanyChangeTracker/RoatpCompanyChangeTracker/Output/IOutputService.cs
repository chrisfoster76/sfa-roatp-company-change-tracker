using RoatpCompanyChangeTracker.Models.Data;
using RoatpCompanyChangeTracker.Models;

namespace RoatpCompanyChangeTracker.Output
{
    internal interface IOutputService
    {
        public void GenerateOutput(List<OutputItem> output, List<RoatpProvider> providers);

    }
}
