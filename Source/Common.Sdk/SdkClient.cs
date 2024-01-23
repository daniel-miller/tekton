using System;
using System.Collections.Generic;
using System.Reflection;

using Common.Contract;

namespace Common.Sdk
{
    public class SdkClient
    {
        private readonly SdkConfiguration _configuration;
        private Pagination _pagination;

        public Pagination Pagination
            => _pagination;

        public SdkClient(SdkConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IEnumerable<T> GetMany<T>(string endpoint, Dictionary<string,string> variables)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.DeveloperSecret))
            {
                var many = api.HttpGet<T>(endpoint, variables);
                _pagination = api.Pagination;
                return many;
            }
        }

        public T GetOne<T>(string endpoint, string item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.DeveloperSecret))
            {
                return api.HttpGet<T>(endpoint, item);
            }
        }

        public T GetOne<T>(string endpoint, Guid item)
            => GetOne<T>(endpoint, item.ToString());

        public T Post<T>(string endpoint, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.DeveloperSecret))
            {
                return api.HttpPost<T>(endpoint, payload);
            }
        }

        public void Post(string endpoint, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.DeveloperSecret))
            {
                api.HttpPost(endpoint, payload);
            }
        }

        public void Delete(string endpoint, string item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.DeveloperSecret))
            {
                api.HttpDelete(endpoint, item);
            }
        }

        public void Delete(string endpoint, Guid item)
            => Delete(endpoint, item.ToString());

        public static Dictionary<string, string> ConvertToDictionary(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            Dictionary<string, string> result = new Dictionary<string, string>();

            PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(obj);

                if (propertyValue != null)
                    result[propertyName] = propertyValue.ToString();
            }

            return result;
        }
    }
}