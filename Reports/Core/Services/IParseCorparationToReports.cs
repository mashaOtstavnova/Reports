using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;

namespace Core.Services
{
    public interface IParseCorparationToReports
    {
        Domain.Reports[] Parse(CorporateNutritionReportItem[] CNRI, DateTime to, DateTime from);
    }
}
