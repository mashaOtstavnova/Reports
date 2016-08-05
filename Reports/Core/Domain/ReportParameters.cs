using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services.Implimintation;
using Newtonsoft.Json;
using Reports;

namespace Core.Domain
{
    /// <summary>
    /// параметры для запроса CorporateNutritionReportItem
    /// </summary>
    public class ReportParameters
    {
        public string OrganizationInfoId;
        public string CorporateNutritionProgramId;
        public DateTime DateFrom;
        public DateTime DateTo;

        public ReportParameters()
        {
            
        }
        public ReportParameters(string organizationInfoId, string corporateNutritionProgramId,
            DateTime dateFrom, DateTime dateTo)
        {
            OrganizationInfoId = organizationInfoId;
            CorporateNutritionProgramId = corporateNutritionProgramId;
            if (dateFrom < dateTo)
            {
                 DateFrom = dateFrom;
                 DateTo= dateTo;
            }
            else
            {
                throw new Exception("Error date in request parameters");
            }
            CoreContext.ReportParametrsService.WriteToFile(this);
        }

       
    }
}
