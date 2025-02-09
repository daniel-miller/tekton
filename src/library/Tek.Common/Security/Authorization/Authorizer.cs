using System;
using System.Collections.Generic;
using System.Linq;

using Tek.Contract;

namespace Tek.Common
{
    public class Authorizer
    {
        private readonly List<Permission> _permissions = new List<Permission>();
        private readonly Dictionary<Guid, Resource> _resources = new Dictionary<Guid, Resource>();

        public string Domain { get; set; }

        public Guid NamespaceId { get; set; }

        public Authorizer(string domain)
        {
            Domain = domain;

            NamespaceId = UuidFactory.CreateV5ForDns(domain);
        }

        public List<Permission> Add(PermissionBundle bundle)
        {
            var list = new List<Permission>();

            if (Enum.TryParse(bundle.Type, out AccessType parsedAccessType))
            {
                var access = new Access();

                access.Type = parsedAccessType;

                switch (parsedAccessType)
                {
                    case AccessType.Basic:
                        if (Enum.TryParse(bundle.Access, out BasicAccess basic))
                            access.Basic = basic;
                            break;

                    case AccessType.Data:
                        if (Enum.TryParse(bundle.Access, out DataAccess data))
                            access.Data = data;
                        break;

                    case AccessType.Http:
                        if (Enum.TryParse(bundle.Access, out HttpAccess http))
                            access.Http = http;
                        break;

                }

                foreach (var resource in bundle.Resources)
                {
                    foreach (var role in bundle.Roles)
                    {
                        var item = Add(resource, role);

                        item.Access = access;

                        list.Add(item);
                    }
                }
            }

            return list;
        }

        public Permission Add(string resourceName, string roleName)
        {
            var resource = Guid.TryParse(resourceName, out Guid resourceId)
                ? new Resource
                {
                    Identifier = resourceId,
                    Name = resourceName,
                }
                : new Resource
                {
                    Identifier = UuidFactory.CreateV5(NamespaceId, resourceName),
                    Name = resourceName,
                };

            var role = Guid.TryParse(roleName, out Guid roleId)
                ? new Role
                {
                    Identifier = roleId,
                    Name = roleName,
                }
                : new Role
                {
                    Identifier = UuidFactory.CreateV5(NamespaceId, roleName),
                    Name = roleName,
                };

            return Add(resource, role);
        }

        public Permission Add(Resource resource, Role role)
        {
            var permission = GetOptional(resource.Identifier, role.Identifier);

            if (permission == null)
            {
                permission = new Permission
                {
                    Access = new Access(),
                    Resource = resource,
                    Role = role
                };

                _permissions.Add(permission);
            }

            if (!_resources.ContainsKey(resource.Identifier))
                _resources.Add(resource.Identifier, resource);

            return permission;
        }

        public void AddResources(Dictionary<string, string> resources)
        {
            RelativeUrlCollection.AddParents(resources);

            foreach (var resourceName in resources.Keys)
            {
                var resource = new Resource
                {
                    Identifier = UuidFactory.CreateV5(NamespaceId, resourceName),
                    Name = resourceName
                };

                if (!_resources.ContainsKey(resource.Identifier))
                    _resources.Add(resource.Identifier, resource);
            }
        }

        public bool Contains(Guid resource, Guid role)
            => GetOptional(resource, role) != null;

        public bool Contains(string resource, string role)
        {
            var resourceId = UuidFactory.CreateV5(NamespaceId, resource);

            var roleId = UuidFactory.CreateV5(NamespaceId, role);

            return Contains(resourceId, roleId);
        }

        private Permission GetOptional(Guid resource, Guid role)
        {
            return _permissions.SingleOrDefault(
              p => p.Resource.Identifier == resource
                && p.Role.Identifier == role);
        }

        private Permission GetOptional(string resource, string role)
        {
            var resourceId = UuidFactory.CreateV5(NamespaceId, resource);

            var roleId = UuidFactory.CreateV5(NamespaceId, role);

            return GetOptional(resourceId, roleId);
        }

        private Permission GetOptional(string resource, Model role)
        {
            var resourceId = UuidFactory.CreateV5(NamespaceId, resource);

            return GetOptional(resourceId, role.Identifier);
        }

        private Permission GetRequired(Guid resource, Guid role)
        {
            var permission = GetOptional(resource, role);

            if (permission == null)
                throw new ArgumentException($"Resource {resource} and Role {role} must be added before granting access.");

            return permission;
        }

        public void Grant(Guid resource, Guid role, BasicAccess access)
        {
            var permission = GetRequired(resource, role);

            permission.Access.Type = AccessType.Basic;
            permission.Access.Basic = access;
            permission.Access.Data = DataAccess.None;
            permission.Access.Http = HttpAccess.None;
        }

        public void Grant(Guid resource, Guid role, DataAccess access)
        {
            var permission = GetRequired(resource, role);

            permission.Access.Type = AccessType.Data;
            permission.Access.Data = access;
            permission.Access.Basic = BasicAccess.Deny;
            permission.Access.Http = HttpAccess.None;
        }

        public void Grant(Guid resource, Guid role, HttpAccess access)
        {
            var permission = GetRequired(resource, role);

            permission.Access.Type = AccessType.Data;
            permission.Access.Http = access;
            permission.Access.Basic = BasicAccess.Deny;
            permission.Access.Data = DataAccess.None;
        }

        public bool IsGranted(Guid resource, IEnumerable<Guid> roles, BasicAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new BasicAccessHelper(permission.Access.Basic);
                
                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(string resource, IEnumerable<Role> roles, BasicAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new BasicAccessHelper(permission.Access.Basic);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(string resource, IEnumerable<Model> roles, BasicAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new BasicAccessHelper(permission.Access.Basic);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(Guid resource, IEnumerable<Guid> roles, DataAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new DataAccessHelper(permission.Access.Data);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(Guid resource, IEnumerable<Guid> roles, HttpAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new HttpAccessHelper(permission.Access.Http);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(string resource, IEnumerable<Model> roles, HttpAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new HttpAccessHelper(permission.Access.Http);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public bool IsGranted(string resource, IEnumerable<string> roles, HttpAccess access)
        {
            foreach (var role in roles)
            {
                var permission = GetOptional(resource, role);

                if (permission == null)
                    continue;

                var helper = new HttpAccessHelper(permission.Access.Http);

                if (helper.IsGranted(access))
                    return true;
            }

            return false;
        }

        public List<Permission> GetPermissions()
            => _permissions;

        public List<Resource> GetResources()
            => _resources.Values.OrderBy(x => x.Name).ToList();
    }
}