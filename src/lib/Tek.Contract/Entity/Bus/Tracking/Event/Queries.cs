using System;
using System.Collections.Generic;

namespace Tek.Contract
{
    public class AssertEvent : Query<bool>
    {
        public Guid EventId { get; set; }
    }

    public class FetchEvent : Query<EventModel>
    {
        public Guid EventId { get; set; }
    }

    public class CollectEvents : Query<IEnumerable<EventModel>>, IEventCriteria
    {
        public Guid AggregateId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }

    public class CountEvents : Query<int>, IEventCriteria
    {
        public Guid AggregateId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }

    public class SearchEvents : Query<IEnumerable<EventMatch>>, IEventCriteria
    {
        public Guid AggregateId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }

    public interface IEventCriteria
    {
        Filter Filter { get; set; }
        
        Guid AggregateId { get; set; }
        Guid OriginId { get; set; }

        string EventData { get; set; }
        string EventType { get; set; }

        int AggregateVersion { get; set; }
    }

    public partial class EventMatch
    {
        public Guid EventId { get; set; }
    }

    public partial class EventModel
    {
        public Guid AggregateId { get; set; }
        public Guid EventId { get; set; }
        public Guid OriginId { get; set; }

        public string EventData { get; set; }
        public string EventType { get; set; }

        public int AggregateVersion { get; set; }
    }
}