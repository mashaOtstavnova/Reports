using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    /// <summary>
    /// параметры для запроса CorporateNutritionReportItem
    /// </summary>
    public class ReportParameters
    {
        public readonly string OrganizationInfoId;
        public readonly string CorporateNutritionProgramId;
        public readonly DateTime DateFrom;
        public readonly DateTime DateTo;

        public ReportParameters(string organizationInfoId, string corporateNutritionProgramId,
            DateTime dateFrom, DateTime dateTo)
        {
            OrganizationInfoId = organizationInfoId;
            CorporateNutritionProgramId = corporateNutritionProgramId;
            DateFrom = dateFrom;
            DateTo= dateTo;
        }

    }
}
