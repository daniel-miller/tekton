using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Common
{
    public static class DictionaryConverter
    {
        public static string ToQueryString(Dictionary<string, string> dictionary)
        {
            if (dictionary == null || dictionary.Count == 0)
                return string.Empty;

            var queryParams = new List<string>();

            foreach (var kvp in dictionary)
            {
                string key = HttpUtility.UrlEncode(kvp.Key);
                string value = HttpUtility.UrlEncode(kvp.Value);
                queryParams.Add($"{key}={value}");
            }

            return string.Join("&", queryParams);
        }

        public static Dictionary<string, string> ToDictionary(object obj)
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
