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
        //public string CorporateNutritionProgramId;
        public DateTime DateFrom;
        public DateTime DateTo;

        public ReportParameters()
        {
            OrganizationInfoId = "7969889b-eaa0-11e5-80d8-d8d38565926f";
            DateFrom = DateTime.Today.AddDays(-2);
            DateTo = DateTime.Today;

        }
        public ReportParameters(string organizationInfoId,
            DateTime dateFrom, DateTime dateTo)/*, string corporateNutritionProgramId,*/
        {
            OrganizationInfoId = organizationInfoId;
            //CorporateNutritionProgramId = corporateNutritionProgramId;
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
