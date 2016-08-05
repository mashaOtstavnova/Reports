using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{

    public abstract class BalancAbstract
    {
        public BalancAbstract( decimal sum)
        {
            Sum = sum;
        }

        public decimal Sum { get; set; }
    }

    public class BalancCardNumber: BalancAbstract
    {
        public BalancCardNumber(string numberCard, decimal sum) : base(sum)
        {
            NumberCard = numberCard;
        }

        public string NumberCard { get; set; }
    }
    public class BalancCategories : BalancAbstract
    {
        public BalancCategories(string nameCategories, decimal sum) : base(sum)
        {
            NameCategories = nameCategories;
        }

        public string NameCategories { get; set; }
    }
}
