using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Domain
{
    public class Card
    {
        public string Id { get; set; }
        public bool IsActivated { get; set; }
        public object NetworkId { get; set; }
        public string Number { get; set; }
        public string OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string Track { get; set; }
    }

    public class Category
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Rank
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
   

    public class WalletBalance
    {
        public double Balance { get; set; }
        public Wallet Wallet { get; set; }
    }

    public class CardInfo
    {
        [JsonProperty("additionalPhones")]
        public CustomerPhone[] AdditionalPhones { get; set; }
        public object Birthday { get; set; }
        public List<Card> Cards { get; set; }
        public List<Category> Categories { get; set; }
        public object Comment { get; set; }
        public string CultureName { get; set; }
        public object Email { get; set; }
        public string Id { get; set; }
        public bool IsBlocked { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Rank Rank { get; set; }
        public int Sex { get; set; }
        public string Surname { get; set; }
        public List<WalletBalance> WalletBalances { get; set; }
    }

    public class CustomerPhone
    {
        public string phone { get; set; }

    }
}
