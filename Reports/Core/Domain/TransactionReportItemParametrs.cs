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
    public class TransactionReportItemParametrs
    {
        public string OrganizationInfoId;
        public DateTime DateFrom;
        public DateTime DateTo;
        public TransactionReportItemParametrs()
        {

        }
        public TransactionReportItemParametrs(string organizationInfoId,
            DateTime dateFrom, DateTime dateTo)
        {
            OrganizationInfoId = organizationInfoId;
            if (dateFrom < dateTo)
            {
                DateFrom = dateFrom;
                DateTo = dateTo;
            }
            else
            {
                throw new Exception("Error date in request parameters");
            }

            //WriteToFile();
        }
        private void WriteToFile()
        {
            Log.Inst.WriteToLogDEBUG(string.Format("Start write Json"));
            var fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            @"iikoReports\TransactionReportItemParametrs.json");
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

            var json = JsonConvert.SerializeObject(new TransactionReportItemParametrs()
            {
                OrganizationInfoId = this.OrganizationInfoId,
                DateFrom = this.DateFrom,
                DateTo = this.DateTo
            }, Formatting.Indented);

            File.WriteAllText(fileName, json);
        }
    }
}
