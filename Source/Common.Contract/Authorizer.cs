using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Contract
{
    public class Authorizer<T>
    {
        private readonly List<Permission<T>> _permissions = new List<Permission<T>>();
        private readonly Func<T,T,bool> _equals;
        private readonly T _root;

        public Authorizer(T root, Func<T,T,bool> equals)
        {
            _root = root;
            _equals = equals;
        }

        public void Add(T operation, T resource, T role)
        {
            if (!IsGranted(operation, resource, role))
                _permissions.Add(new Permission<T> { Operation = operation, Resource = resource, Role = role });
        }

        public bool IsGranted(T operation, T resource, T role)
        {
            return _permissions.Any(rule => _equals(rule.Operation, operation) && _equals(rule.Resource, resource) && _equals(rule.Role, role));
        }

        public bool IsGranted(T operation, T resource, IEnumerable<T> roles)
        {
            if (IsRoot(roles))
                return true;

            if (!_permissions.Any(rule => _equals(rule.Resource, resource) && _equals(rule.Operation, operation)))
                return false;

            var grants = _permissions
                .Where(rule => _equals(rule.Resource, resource) && _equals(rule.Operation, operation))
                .Select(rule => rule.Role);

            return roles.Intersect(grants).Any();
        }

        private bool IsRoot(IEnumerable<T> roles)
        {
            return roles.Any(role => _equals(role, _root));
        }
    }
}