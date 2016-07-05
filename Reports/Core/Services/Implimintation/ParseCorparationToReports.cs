using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain;
using Core.Services;

namespace Core.Services.Implimintation
{
    /// <summary>
    /// парсинг ответа в сущность для отображения на экран
    /// </summary>
    public class ParseCorparationToReports:IParseCorparationToReports
    {
        public Domain.Reports[] Parse(CorporateNutritionReportItem[] CNRI, DateTime to, DateTime from)
        {
            List<Domain.Reports> listReportses = new List<Domain.Reports>();
            foreach (var item in CNRI)
            {
                listReportses.Add(new Domain.Reports()
                {
                    BalanceOnPeriodEnd = item.BalanceOnPeriodEnd,
                    BalanceOnPeriodStart = item.BalanceOnPeriodStart,
                    To = to,
                    From = from,
                    PaidOrdersCount = item.PaidOrdersCount,
                    GuestName = item.GuestName,
                    BalanceResetSum = item.BalanceResetSum,
                    BalanceRefillSum = item.BalanceRefillSum,
                    GuestCardTrack = item.GuestCardTrack,
                    GuestCategoryNames = item.GuestCategoryNames,
                    EmployeeNumber = item.EmployeeNumber,
                    GuestId = item.GuestId,
                    PayFromWalletSum = item.PayFromWalletSum,
                    GuestPhone = item.GuestPhone
                });
            }
            return listReportses.ToArray();
        }
    }
}
