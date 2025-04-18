using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertProvince : Query<bool>
    {
        public Guid ProvinceId { get; set; }
    }

    public class FetchProvince : Query<ProvinceModel>
    {
        public Guid ProvinceId { get; set; }
    }

    public class CollectProvinces : Query<IEnumerable<ProvinceModel>>, IProvinceCriteria
    {
        public Guid? CountryId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }

    public class CountProvinces : Query<int>, IProvinceCriteria
    {
        public Guid? CountryId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }

    public class SearchProvinces : Query<IEnumerable<ProvinceMatch>>, IProvinceCriteria
    {
        public Guid? CountryId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }

    public interface IProvinceCriteria
    {
        Filter Filter { get; set; }
        
        Guid? CountryId { get; set; }

        string CountryCode { get; set; }
        string ProvinceCode { get; set; }
        string ProvinceName { get; set; }
    }

    public partial class ProvinceMatch
    {
        public Guid ProvinceId { get; set; }
    }

    public partial class ProvinceModel
    {
        public Guid? CountryId { get; set; }
        public Guid ProvinceId { get; set; }

        public string CountryCode { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
    }
}