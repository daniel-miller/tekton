namespace Tek.Contract
{
    public static partial class Endpoints
    {
        public static partial class BusApi
        {
            public static partial class Tracking
            {
                public const string Name = "Bus: Tracking";

                public static class Aggregate
                {
                    private const string Collection = "bus/aggregates";
                    private const string Item = "/{aggregate:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Event
                {
                    private const string Collection = "bus/events";
                    private const string Item = "/{event:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }
        }

        public static partial class ContentApi
        {
            public static partial class Text
            {
                public const string Name = "Content: Text";

                public static class Translation
                {
                    private const string Collection = "content/translations";
                    private const string Item = "/{translation:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }
        }

        public static partial class LocationApi
        {
            public static partial class Region
            {
                public const string Name = "Location: Region";

                public static class Country
                {
                    private const string Collection = "location/countries";
                    private const string Item = "/{country:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Province
                {
                    private const string Collection = "location/provinces";
                    private const string Item = "/{province:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }
        }

        public static partial class MetadataApi
        {
            public static partial class Audit
            {
                public const string Name = "Metadata: Audit";

                public static class Origin
                {
                    private const string Collection = "metadata/origins";
                    private const string Item = "/{origin:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Version
                {
                    private const string Collection = "metadata/versions";
                    private const string Item = "/{version:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }

            public static partial class Storage
            {
                public const string Name = "Metadata: Storage";

                public static class Entity
                {
                    private const string Collection = "metadata/entities";
                    private const string Item = "/{entity:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }
        }

        public static partial class SecurityApi
        {
            public static partial class Authorization
            {
                public const string Name = "Security: Authorization";

                public static class Permission
                {
                    private const string Collection = "security/permissions";
                    private const string Item = "/{permission:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Resource
                {
                    private const string Collection = "security/resources";
                    private const string Item = "/{resource:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }

            public static partial class Identification
            {
                public const string Name = "Security: Identification";

                public static class Organization
                {
                    private const string Collection = "security/organizations";
                    private const string Item = "/{organization:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Partition
                {
                    private const string Collection = "security/partitions";
                    private const string Item = "/{partition:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Password
                {
                    private const string Collection = "security/passwords";
                    private const string Item = "/{password:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Role
                {
                    private const string Collection = "security/roles";
                    private const string Item = "/{role:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }

                public static class Secret
                {
                    private const string Collection = "security/secrets";
                    private const string Item = "/{secret:guid}";

                    // Queries

                    public const string Assert  = Collection + Item; // HEAD : Check for a single item
                    public const string Fetch   = Collection + Item; // GET  : Retrieve a single item
                    
                    public const string Collect = Collection;             // GET or POST : Retrieve multiple items
                    public const string Count   = Collection + "/count";  // GET or POST : Count multiple items
                    public const string Search  = Collection + "/search"; // GET or POST : Find multiple items

                    // Commands

                    public const string Create = Collection;        // POST   : Insert a single item
                    public const string Delete = Collection + Item; // DELETE : Delete a single item
                    public const string Modify = Collection + Item; // PUT    : Update a single item
                }
            }
        }

    }
}