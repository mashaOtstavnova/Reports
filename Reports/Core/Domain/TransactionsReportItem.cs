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
        public string OrderNumber { get; set; } //не long так как иногда из данных приходит null и десериализация не срабатывает
        public decimal OrderSum { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime TransactionCreateDate { get; set; }
        public decimal TransactionSum { get; set; }
        public string TransactionType { get; set; }
    }
}
