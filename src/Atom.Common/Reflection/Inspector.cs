using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atom.Common
{
    public class Inspector
    {
        public static Dictionary<string, string> CreateDictionary(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null.");

            var result = new Dictionary<string, string>();

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
