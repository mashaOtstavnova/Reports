using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Services
{
    public interface IMakerRequest
    {

        Core.Domain.Reports[] GetReportses(ReportParameters param);
        Task<CorporateNutritionInfo[]> GetCorporateNutritionInfo(string organizationId);
        Task<OrganizationInfo[]> GetOrganizationInfo();
    }
}
