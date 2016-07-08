using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        { }
        public ReportParameters(string organizationInfoId, string corporateNutritionProgramId,
            DateTime dateFrom, DateTime dateTo)
        {
            OrganizationInfoId = organizationInfoId;
            CorporateNutritionProgramId = corporateNutritionProgramId;
            DateFrom = dateFrom;
            DateTo= dateTo;
        }

        public void WriteToFile()
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start write Json"));
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"iikoReports\ReportParameters.json");
            var tmp = Directory.Exists("iikoReports");
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"iikoReports")))
            {
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"iikoReports"));
            }
            if (!File.Exists(fileName))
            {
                File.Create(fileName);
            }
            
            var json = JsonConvert.SerializeObject(new ReportParameters()
            {
                CorporateNutritionProgramId = this.CorporateNutritionProgramId,
                OrganizationInfoId = this.OrganizationInfoId,
                DateFrom = this.DateFrom,
                DateTo = this.DateTo
            }, Formatting.Indented);

            File.WriteAllText(fileName, json);
        }
    }
}
