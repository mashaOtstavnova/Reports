using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class ColumnTableView
    {
        public DateTime DateTime { get; set; }//resultTable.Columns.Add("Дата/время", typeof (DateTime));
        public DateTime Date { get; set; } //    resultTable.Columns.Add("Дата", typeof (DateTime));
        public string NumberCard { get; set; }//    resultTable.Columns.Add("Код карты", dt.Columns["CardNumbers"].DataType);
        public string FullName { get; set; }//    resultTable.Columns.Add("ФИО", typeof (string));
        public string Category { get; set; }//    resultTable.Columns.Add("Категория", typeof (string));
        public string SumTranzaction { get; set; }//    resultTable.Columns.Add("Сумма чека", dt.Columns["TransactionSum"].DataType);
        public string Balance { get; set; }//    resultTable.Columns.Add("Дотация", typeof (decimal));
        public decimal Credit { get; set; } //    resultTable.Columns.Add("Кредит", typeof (decimal));
        public string PhoneName { get; set; }//    resultTable.Columns.Add("Телефон", dt.Columns["PhoneNumber"].DataType);
    }
}
