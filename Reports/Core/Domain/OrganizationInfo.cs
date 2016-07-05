using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class OrganizationInfo
    {
        public object Address { get; set; }
        public object AverageCheque { get; set; }
        public ContactInfo Contact { get; set; }
        public string Description { get; set; }
        public object HomePage { get; set; }
        public string Id { get; set; }
        public bool IsActive { get; set; }
        public object Latitude { get; set; }
        public object Logo { get; set; }
        public object LogoImage { get; set; }
        public object Longitude { get; set; }
        public int MaxBonus { get; set; }
        public int MinBonus { get; set; }
        public string Name { get; set; }
        public object NetworkId { get; set; }
        public int OrganizationType { get; set; }
        public object Phone { get; set; }
        public object Website { get; set; }
        public object WorkTime { get; set; }
        public string CurrencyIsoName { get; set; }

    }
    public class ContactInfo
    {
        public string email { get; set; }
        public string location { get; set; }
        public object phone { get; set; }
    }
}
