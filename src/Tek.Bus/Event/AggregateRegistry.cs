using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tek.Bus
{
    public static class AggregateRegistry
    {
        private static IAggregateTypeInfo[] _aggregates = null;
        private static IReadOnlyDictionary<string, Type> _events = null;

        public static void Initialize(string assemblyNameStartsWith)
        {
            var types = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.FullName.StartsWith(assemblyNameStartsWith))
                .SelectMany(s => s.GetTypes())
                .ToArray();

            _events = GetEvents(types);
            _aggregates = GetAggregateTypes(types);
        }

        private static AggregateTypeInfo[] GetAggregateTypes(Type[] types)
        {
            var aggregateRootType = typeof(AggregateRoot);

            var list = new List<AggregateTypeInfo>();

            for (var i = 0; i < types.Length; i++)
            {
                var t = types[i];
                if (!aggregateRootType.IsAssignableFrom(t))
                    continue;

                var dataProperty = t.GetProperty("Data");
                if (dataProperty == null)
                    continue;

                var aggregateTypeInfo = new AggregateTypeInfo(t);

                aggregateTypeInfo.Events = GetEventTypes(dataProperty.PropertyType, aggregateTypeInfo);

                list.Add(aggregateTypeInfo);
            }

            return list.OrderBy(x => x.Name).ToArray();
        }

        private static EventTypeInfo[] GetEventTypes(Type aggregateType, AggregateTypeInfo aggInfo)
        {
            var eventType = typeof(Event);

            var list = new List<EventTypeInfo>();

            var aggregateMethods = aggregateType
                .GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (var i = 0; i < aggregateMethods.Length; i++)
            {
                var m = aggregateMethods[i];
                if (m.Name != "When")
                    continue;

                var parameters = m.GetParameters();
                if (parameters.Length != 1)
                    continue;

                var parameterType = parameters[0].ParameterType;
                if (!eventType.IsAssignableFrom(parameterType))
                    continue;

                list.Add(new EventTypeInfo(parameterType, aggInfo));
            }

            return list.OrderBy(x => x.Type.Name).ToArray();
        }

        public static IAggregateTypeInfo[] GetAggregates() => _aggregates;

        public static Type GetEventType(string name)
        {
            if (_events.TryGetValue(name, out var result))
                return result;

            throw new EventClassNotFoundException(name);
        }

        private static IReadOnlyDictionary<string, Type> GetEvents(Type[] types)
        {
            var eventType = typeof(Event);

            try
            {
                var list = new Dictionary<string, Type>();

                var duplicates = new List<string>();

                for (var i = 0; i < types.Length; i++)
                {
                    var t = types[i];
                    if (!t.IsSubclassOf(eventType))
                        continue;

                    if (list.ContainsKey(t.Name))
                        duplicates.Add(t.Name);
                    else
                        list.Add(t.Name, t);
                }

                if (duplicates.Count > 0)
                {
                    var names = string.Join(", ", duplicates.OrderBy(x => x).Distinct());

                    throw new DuplicateEventTypeNamesException(names);
                }

                return new ReadOnlyDictionary<string, Type>(list);
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();

                foreach (var exSub in ex.LoaderExceptions)
                {
                    sb.AppendLine(exSub.Message);

                    if (exSub is FileNotFoundException exFileNotFound)
                    {
                        if (!string.IsNullOrWhiteSpace(exFileNotFound.FusionLog))
                        {
                            sb.AppendLine("Fusion Log:");
                            sb.AppendLine(exFileNotFound.FusionLog);
                        }
                    }

                    sb.AppendLine();
                }

                var errorMessage = sb.ToString();

                // Display or log the error based on your application.
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }
    }
}