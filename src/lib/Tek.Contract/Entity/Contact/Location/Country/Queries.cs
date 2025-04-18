using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertCountry : Query<bool>
    {
        public Guid CountryId { get; set; }
    }

    public class FetchCountry : Query<CountryModel>
    {
        public Guid CountryId { get; set; }
    }

    public class CollectCountries : Query<IEnumerable<CountryModel>>, ICountryCriteria
    {
        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class CountCountries : Query<int>, ICountryCriteria
    {
        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public class SearchCountries : Query<IEnumerable<CountryMatch>>, ICountryCriteria
    {
        public string CapitalCityName { get; set; }
        public string ContinentCode { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string Languages { get; set; }
        public string TopLevelDomain { get; set; }
    }

    public interface ICountryCriteria
    {
        Filter Filter { get; set; }
        
        string CapitalCityName { get; set; }
        string ContinentCode { get; set; }
        string CountryCode { get; set; }
        string CountryName { get; set; }
        string CurrencyCode { get; set; }
        string CurrencyName { get; set; }
        string Languages { get; set; }
        string TopLevelDomain { get; set; }
    }

    public partial class CountryMatch
    {
        public Guid CountryId { get; set; }
    }

    public partial class CountryModel
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
}