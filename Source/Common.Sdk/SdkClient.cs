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

        public IEnumerable<T> GetList<T>(string endpoint, Dictionary<string,string> variables)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                var many = api.HttpGet<T>(endpoint, variables);
                _pagination = api.Pagination;
                return many;
            }
        }

        public T GetItem<T>(string endpoint, string item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                return api.HttpGet<T>(endpoint, item);
            }
        }

        public T GetItem<T>(string endpoint, string[] item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                return api.HttpGet<T>(endpoint, item);
            }
        }

        public T GetItem<T>(string endpoint, Guid item)
            => GetItem<T>(endpoint, item.ToString());

        public T GetItem<T>(string endpoint, Guid item1, Guid item2)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString() });

        public T GetItem<T>(string endpoint, Guid item1, Guid item2, Guid item3)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public T GetItem<T>(string endpoint, Guid item1, Guid item2, int item3)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public T GetItem<T>(string endpoint, Guid item1, int item2)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2.ToString() });

        public T GetItem<T>(string endpoint, Guid item1, string item2)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2 });

        public T GetItem<T>(string endpoint, Guid item1, string item2, int item3)
            => GetItem<T>(endpoint, new[] { item1.ToString(), item2, item3.ToString() });

        public T GetItem<T>(string endpoint, int item)
            => GetItem<T>(endpoint, item.ToString());

        public T Post<T>(string endpoint, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                return api.HttpPost<T>(endpoint, payload);
            }
        }

        public void Post(string endpoint, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                api.HttpPost(endpoint, payload);
            }
        }

        public T Put<T>(string endpoint, Guid item, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                return api.HttpPut<T>(endpoint, item.ToString(), payload);
            }
        }

        public void Put(string endpoint, Guid item, object payload)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                api.HttpPut(endpoint, item.ToString(), payload);
            }
        }


        public void Delete(string endpoint, string item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                api.HttpDelete(endpoint, item);
            }
        }

        public void Delete(string endpoint, string[] item)
        {
            using (var api = new ApiClient(_configuration.ApiUrl, _configuration.TokenSecret))
            {
                api.HttpDelete(endpoint, item);
            }
        }

        public void Delete(string endpoint, Guid item)
            => Delete(endpoint, item.ToString());

        public void Delete(string endpoint, Guid item1, Guid item2)
            => Delete(endpoint, new[] { item1.ToString(), item2.ToString() });

        public void Delete(string endpoint, Guid item1, Guid item2, Guid item3)
            => Delete(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public void Delete(string endpoint, Guid item1, Guid item2, int item3)
            => Delete(endpoint, new[] { item1.ToString(), item2.ToString(), item3.ToString() });

        public void Delete(string endpoint, Guid item1, int item2)
            => Delete(endpoint, new[] { item1.ToString(), item2.ToString() });

        public void Delete(string endpoint, Guid item1, string item2)
            => Delete(endpoint, new[] { item1.ToString(), item2 });

        public void Delete(string endpoint, Guid item1, string item2, int item3)
            => Delete(endpoint, new[] { item1.ToString(), item2, item3.ToString() });

        public void Delete(string endpoint, int item)
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