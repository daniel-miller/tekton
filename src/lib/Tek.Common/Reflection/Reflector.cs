using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tek.Common
{
    public class Reflector
    {
        public Dictionary<string, string> CreateDictionary(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Object cannot be null.");
            }

            var result = new Dictionary<string, string>();

            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                var propertyName = property.Name;

                var propertyValue = property.GetValue(obj);

                if (propertyValue != null)
                {
                    result[propertyName] = propertyValue.ToString();
                }
            }

            return result;
        }

        public void FindConstants(Type type, Dictionary<string, string> dictionary)
        {
            FindConstants(type, "*", dictionary);
        }

        public void FindConstants(Type type, IEnumerable<string> constants, Dictionary<string, string> dictionary)
        {
            foreach (var constant in constants)
            {
                FindConstants(type, constant, dictionary);
            }
        }

        public void FindConstants(Type type, string constant, Dictionary<string, string> dictionary)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field != null && (constant == "*" || constant == field.Name) && field.IsLiteral)
                {
                    var fieldType = field.DeclaringType?.FullName;

                    var fieldValue = field.GetValue(null)?.ToString();

                    if (fieldType != null && fieldValue != null)
                    {
                        var key = fieldType.Replace("+", ".") + "." + field.Name;

                        dictionary.Add(key, fieldValue);
                    }
                }
            }

            foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.Static))
            {
                FindConstants(nestedType, constant, dictionary);
            }
        }

        public Dictionary<string, string> FindRelativeUrls(Type type)
        {
            var relativeUrls = new Dictionary<string, string>();

            var constantUrls = new Dictionary<string, string>();

            FindConstants(type, constantUrls);

            foreach (var constantName in constantUrls.Keys)
            {
                var constantValue = constantUrls[constantName];

                if (!relativeUrls.ContainsKey(constantValue))
                    if (!constantName.EndsWith(".Name"))
                        relativeUrls.Add(constantValue, constantName);
            }

            RelativeUrlCollection.AddParents(relativeUrls);

            return relativeUrls;
        }

        /// <summary>
        /// Returns the assembly-qualified class name without the version, culture, and public key token.
        /// </summary>
        public string GetClassName(Type type)
        {
            return $"{type.FullName}, {Assembly.GetAssembly(type).GetName().Name}";
        }

        /// <summary>
        /// Convert the type name (including namespace) to a kebab-case relative path.
        /// </summary>
        public string GetResourceName(Type type)
        {
            var className = type.FullName;
            var title = className.Replace(".", "/");
            var kebab = title.ToKebabCase();
            return kebab;
        }
    }
}