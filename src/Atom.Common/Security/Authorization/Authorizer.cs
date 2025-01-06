using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Common
{
    public class Authorizer
    {
        private readonly List<Permission> _permissions = new List<Permission>();
        private readonly Guid _root;

        public Authorizer(Guid root)
        {
            _root = root;
        }

        public void Add(Function function, Resource resource, Guid role)
        {
            if (IsGranted(function.Identifier, resource.Identifier, role))
                return;

            var permission = new Permission
            {
                Function = function,
                Resource = resource,
                Role = new Role { Identifier = role }
            };

            _permissions.Add(permission);
        }

        public void Add(Function function, Guid resource, Guid role)
        {
            Add(function, new Resource { Identifier = resource }, role);
        }

        public void Add(Guid function, Guid resource, Guid role)
        {
            Add(new Function { Identifier = function }, resource, role);
        }

        public bool IsGranted(string function, string resource, IEnumerable<Guid> roles)
        {
            if (IsRoot(roles))
                return true;

            return _permissions.Any(rule => rule.Function.Slug == function && rule.Resource.Slug == resource && roles.Any(role => rule.Role.Identifier == role));
        }

        public bool IsGranted(string function, string resource, Guid role)
        {
            if (IsRoot(role))
                return true;

            return _permissions.Any(rule => rule.Function.Slug == function && rule.Resource.Slug == resource && rule.Role.Identifier == role);
        }

        public bool IsGranted(Guid function, Guid resource, Guid role)
        {
            if (IsRoot(role))
                return true;

            return _permissions.Any(rule => rule.Function.Identifier == function && rule.Resource.Identifier == resource && rule.Role.Identifier == role);
        }

        public bool IsGranted(Guid function, Guid resource, IEnumerable<Guid> roles)
        {
            if (IsRoot(roles))
                return true;

            if (!_permissions.Any(rule => rule.Resource.Identifier == resource && rule.Function.Identifier == function))
                return false;

            var grants = _permissions
                .Where(rule => rule.Resource.Identifier == resource && rule.Function.Identifier == function)
                .Select(rule => rule.Role.Identifier);

            return roles.Intersect(grants).Any();
        }

        private bool IsRoot(Guid role)
        {
            return role == _root;
        }

        private bool IsRoot(IEnumerable<Guid> roles)
        {
            return roles.Any(role => role == _root);
        }
    }
}