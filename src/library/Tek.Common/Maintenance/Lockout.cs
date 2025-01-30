using System;
using System.Collections.Generic;
using System.Linq;

using Tek.Contract;

namespace Tek.Common
{
    public class Lockout : Model
    {
        /// <summary>
        /// The enterprises to which this lockout applies.
        /// </summary>
        public string[] Enterprises { get; set; }

        /// <summary>
        /// The environments to which this lockout applies.
        /// </summary>
        public string[] Environments { get; set; }

        /// <summary>
        /// The interfaces to which this lockout applies.
        /// </summary>
        public string[] Interfaces { get; set; }

        /// <summary>
        /// The interval of time during which the lockout is in effect.
        /// </summary>
        public Interval Interval { get; set; }

        public Lockout()
        {
            Interval = new Interval();
            Enterprises = Environments = Interfaces = new string[0];
        }

        public bool FilterEnterprises()
            => Enterprises != null && Enterprises.Any();

        public bool FilterEnvironments()
            => Environments != null && Environments.Any();

        public bool FilterInterfaces()
            => Interfaces != null && Interfaces.Any();

        public int? MinutesBeforeOpenTime(DateTimeOffset current, string enterprise, string environment)
        {
            var next = NextOpenTime(current, enterprise, environment);

            if (next == null)
                return null;

            return Interval.MinutesBeforeOpenTime(current);
        }

        public DateTimeOffset? NextOpenTime(DateTimeOffset current, string enterprise, string environment)
        {
            var next = Interval.NextOpenTime(current);

            if (next < current)
                return null;

            if (FilterEnterprises() && enterprise.MatchesNone(Enterprises))
                return null;

            if (FilterEnvironments() && environment.MatchesNone(Environments))
                return null;

            return next;
        }

        public bool IsActive(DateTimeOffset current, string enterprise, string environment)
        {
            if (!Interval.Contains(current))
                return false;

            if (FilterEnterprises() && enterprise.MatchesNone(Enterprises))
                return false;

            if (FilterEnvironments() && environment.MatchesNone(Environments))
                return false;

            return true;
        }

        public bool IsValid()
            => !Validate().Any();

        public IEnumerable<ValidationError> Validate()
        {
            var errors = Interval.Validate().ToList();

            foreach (var i in Environments)
                if (i.MatchesNone(Contract.Environments.Names))
                    errors.Add(new ValidationError { Property = nameof(Environments), Summary = $"Environments can contain only items in this list: {string.Join("; ", Contract.Environments.Names)}" });

            foreach (var i in Interfaces)
                if (i.MatchesNone(new[] { "api", "ui" }))
                    errors.Add(new ValidationError { Property = nameof(Environments), Summary = "Interfaces can contain only items in this list: API; UI" });

            return errors;
        }
    }
}