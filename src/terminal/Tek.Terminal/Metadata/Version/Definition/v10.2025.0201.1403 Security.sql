CREATE TABLE security.t_partition (

    partition_number INT NOT NULL PRIMARY KEY,
    partition_slug VARCHAR(3) NOT NULL,
    partition_name VARCHAR(50) NOT NULL,
    partition_host VARCHAR(50) NOT NULL,
    partition_email VARCHAR(254) NOT NULL,
    partition_settings JSON NULL,
    partition_testers VARCHAR(400) NULL,

    modified_when TIMESTAMPTZ NOT NULL
);

CREATE TABLE security.t_organization (

    organization_id UUID NOT NULL PRIMARY KEY,
    
    organization_number INT NOT NULL,
    organization_slug VARCHAR(3) NOT NULL,
    organization_name VARCHAR(50) NOT NULL,
    
    partition_number INT NOT NULL,

    modified_when TIMESTAMPTZ NOT NULL
);

CREATE TABLE security.t_password (

    password_id UUID NOT NULL PRIMARY KEY,

    email_id UUID NOT NULL,          -- v5 UUID for email address (remember this changes if the email address changes)
    email_address VARCHAR(254) NULL, -- optionally store the email address

    password_hash VARCHAR(100) NOT NULL,
    password_expiry TIMESTAMPTZ NOT NULL,
    
    default_plaintext VARCHAR(100) NULL,
    default_expiry TIMESTAMPTZ NULL,
    
    created_when TIMESTAMPTZ NOT NULL,
    
    last_forgotten_when TIMESTAMPTZ NULL,
    last_modified_when TIMESTAMPTZ NULL
);

CREATE TABLE security.t_role (

    role_id UUID NOT NULL PRIMARY KEY,
    
    role_type VARCHAR(20) NOT NULL, -- Organization or Partition
    role_name VARCHAR(100) NOT NULL
);

CREATE TABLE security.t_resource (

    resource_id UUID NOT NULL PRIMARY KEY,
    
    resource_type VARCHAR(30) NOT NULL,
    resource_name VARCHAR(100) NOT NULL
);

CREATE TABLE security.t_permission (

    permission_id UUID NOT NULL PRIMARY KEY,

    access_type VARCHAR(10) NOT NULL, -- Basic | Data | Http | System
    access_flags INT NOT NULL,

    resource_id UUID NOT NULL,
    role_id UUID NOT NULL
);