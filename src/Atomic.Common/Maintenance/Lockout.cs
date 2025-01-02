using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomic.Common
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
        /// The interval of time during which the lockout is in effect.
        /// </summary>
        public Interval Interval { get; set; }

        public Lockout()
        {
            Interval = new Interval();
        }

        public bool FilterEnterprises()
            => Enterprises != null && Enterprises.Any();

        public bool FilterEnvironments()
            => Environments != null && Environments.Any();

        public int? MinutesUntilNextActualStartTime(DateTimeOffset when, string enterprise, string environment)
        {
            var next = NextActualStartTime(when, enterprise, environment);

            if (next == null)
                return null;

            return (int)(next.Value - when).TotalMinutes;
        }

        public DateTimeOffset? NextActualStartTime(DateTimeOffset when, string enterprise, string environment)
        {
            var next = Interval.NextOpenTime(when);

            if (next < when)
                return null;

            if (FilterEnterprises() && !AreEqual(enterprise, Enterprises))
                return null;

            if (FilterEnvironments() && !AreEqual(environment, Environments))
                return null;

            return next;
        }

        public bool IsActive(DateTimeOffset when, string enterprise, string environment)
        {
            if (!Interval.Contains(when))
                return false;

            if (FilterEnterprises() && !AreEqual(enterprise, Enterprises))
                return false;

            if (FilterEnvironments() && !AreEqual(environment, Environments))
                return false;

            return true;
        }

        public bool IsValid()
            => !Validate().Any();

        public IEnumerable<ValidationError> Validate()
        {
            var errors = Interval.Validate().ToList();

            if (Environments == null)
                errors.Add(new ValidationError { Property = nameof(Environments), Summary = "Environments cannot be null. Use an empty list for all environments." });

            if (Enterprises == null)
                errors.Add(new ValidationError { Property = nameof(Enterprises), Summary = "Enterprises cannot be null. Use an empty list for all enterprises." });

            return errors;
        }
        
        private bool AreEqual(string x, string y)
            => string.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
        
        private bool AreEqual(string value, string[] list)
            => list.Any(item => AreEqual(item, value));
    }
}