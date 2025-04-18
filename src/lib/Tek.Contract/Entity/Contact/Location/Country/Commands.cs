using System;

namespace Tek.Contract
{
    public class CreateCountry
    {
        public Guid CountryId { get; set; }

        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class ModifyCountry
    {
        public Guid CountryId { get; set; }

        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class DeleteCountry
    {
        public Guid CountryId { get; set; }
    }
}