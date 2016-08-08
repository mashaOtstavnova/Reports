using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Config
    {
        public List<BalancCategories> BalanceDictionary { get; set; }
        public string Version { get; set; }
        public Config()
        {
            Version = "1.0";

            BalanceDictionary = new List<BalancCategories>();
            BalanceDictionary.Add(new BalancCategories("АкТай", 250));
            BalanceDictionary.Add(new BalancCategories("АкТай кредит", 350));
            BalanceDictionary.Add(new BalancCategories("АкТай лимит 250", 450));
        }
    }
}
