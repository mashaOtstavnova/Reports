using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Newtonsoft.Json;
using Reports;

namespace Core.Services.Implimintation
{
    public class ReportParametrsService:IReportParametrsService
    {
        private string _fileName;
        private string _filePatch;

        public ReportParametrsService()
        {
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"iikoReports\ReportParameters.json");
            _filePatch= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"iikoReports");
        }
        public void WriteToFile(ReportParameters reportParameters)
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start write Json"));
           
            if (!Directory.Exists(_filePatch))
            {
                Directory.CreateDirectory(_filePatch);
            }
            if (!File.Exists(_fileName))
            {
                File.Create(_fileName);
            }

            var json = JsonConvert.SerializeObject(new ReportParameters()
            {
                //CorporateNutritionProgramId = reportParameters.CorporateNutritionProgramId,
                OrganizationInfoId = reportParameters.OrganizationInfoId,
                DateFrom = reportParameters.DateFrom,
                DateTo = reportParameters.DateTo
            }, Formatting.Indented);

            File.WriteAllText(_fileName, json);
        }

        public ReportParameters GetSettings()
        {
            if (Directory.Exists(_filePatch))
            {
                if (File.Exists(_fileName))
                {
                    var json = File.ReadAllText(_fileName);
                    var configThis = JsonConvert.DeserializeObject<ReportParameters>(json);
                    return configThis;
                }
            }
            return null;
        }
    }
}
