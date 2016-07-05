using System.Collections.Generic;

namespace Core.Domain
{
    /// <summary>
    /// Информация о программе корпоративного питания
    /// </summary>

    public class CorporateNutritionInfo
    {
        public object Description { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string ServiceFrom { get; set; }
        public object ServiceTo { get; set; }
        public List<Wallet> Wallets { get; set; }
    }

    public class Wallet
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}