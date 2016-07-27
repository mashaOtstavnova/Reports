using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class TransactionsReportItem
    {
        public string CardNumbers { get; set; }
        public string OrderCreateDate { get; set; }
        public decimal OrderNumber { get; set; }
        public decimal OrderSum { get; set; }
        public string PhoneNumber { get; set; }
        public string TransactionCreateDate { get; set; }
        public decimal TransactionSum { get; set; }
        public string TransactionType { get; set; }
    }
}
