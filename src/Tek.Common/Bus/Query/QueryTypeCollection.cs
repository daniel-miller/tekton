using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Tek.Contract;

namespace Tek.Common
{
    public class QueryTypeCollection
    {
        private readonly Dictionary<string, Type> _queries;
        private readonly Dictionary<Type, Type> _results;

        public QueryTypeCollection(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            _queries = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && IsSubclassOfRawGeneric(typeof(Query<>), t))
                .ToDictionary(t => t.Name.ToLower(), t => t);

            _results = new Dictionary<Type, Type>();

            foreach (var key in _queries.Keys)
            {
                var query = _queries[key];

                var iQueryInterface = query.GetInterfaces()
                    .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));

                if (iQueryInterface == null)
                    throw new Exception($"{key} does not implement the IQuery interface.");

                var resultType = iQueryInterface.GetGenericArguments()[0];

                _results.Add(query, resultType);
            }
        }

        private bool IsSubclassOfRawGeneric(Type generic, Type type)
        {
            while (type != null && type != typeof(object))
            {
                var current = type.IsGenericType ? type.GetGenericTypeDefinition() : type;
                if (generic == current)
                    return true;

                type = type.BaseType;
            }
            return false;
        }

        public Type GetQueryType(string name)
        {
            var typeNameVariations = GetTypeNameVariations(name);

            foreach (var typeName in typeNameVariations)
            {
                if (_queries.TryGetValue(typeName, out Type type))
                {
                    return type;
                }
            }

            return null;
        }

        private List<string> GetTypeNameVariations(string typeName)
        {
            var list = new List<string>();

            if (string.IsNullOrWhiteSpace(typeName))
                return list;

            list.Add(RemoveHyphens(typeName));

            const string prefix = "Query";

            if (!typeName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                list.Add($"Query{RemoveHyphens(typeName)}");
            }

            for (var i = 0; i < list.Count; i++)
            {
                list[i] = list[i].Trim().ToLower();
            }

            return list;
        }

        public static string RemoveHyphens(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Replace("-", string.Empty);
        }

        public Type GetResultType(Type query)
        {
            if (_results.TryGetValue(query, out Type type))
            {
                return type;
            }

            return null;
        }
    }
}