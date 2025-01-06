using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atom.Common
{
    public class Reflector
    {
        public void FindConstants(Type type, IEnumerable<string> constants, Dictionary<string, string> dictionary)
        {
            foreach (var constant in constants)
            {
                FindConstants(type, constant, dictionary);
            }
        }

        public void FindConstants(Type type, string constant, Dictionary<string, string> dictionary)
        {
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);

            foreach (var field in fields)
            {
                if (field != null && field.Name == constant && field.IsLiteral)
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

            foreach (var nestedType in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            {
                FindConstants(nestedType, constant, dictionary);
            }
        }

        /// <summary>
        /// Returns the assembly-qualified class name without the version, culture, and public key token.
        /// </summary>
        public string GetClassName(Type type)
        {
            return $"{type.FullName}, {Assembly.GetAssembly(type).GetName().Name}";
        }
    }
}