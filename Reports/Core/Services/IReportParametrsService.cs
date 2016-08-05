using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Services
{
    public interface IReportParametrsService
    {
        void WriteToFile(ReportParameters reportParameters);
        ReportParameters GetSettings();
    }
}
