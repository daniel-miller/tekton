-- Create a table to store entity metadata.

CREATE TABLE metadata.t_entity (

  storage_structure VARCHAR(20) NOT NULL,
  storage_schema VARCHAR(30) NOT NULL,
  storage_table VARCHAR(40) NOT NULL,
  storage_table_future VARCHAR(40) NULL,
  storage_key VARCHAR(80) NOT NULL,

  component_type VARCHAR(20) NOT NULL,
  component_name VARCHAR(30) NOT NULL,
  component_feature VARCHAR(40) NOT NULL,

  entity_name VARCHAR(50) NOT NULL,

  collection_slug VARCHAR(50) NOT NULL,
  collection_key VARCHAR(60) NOT NULL,

  entity_id UUID NOT NULL PRIMARY KEY

);

-- Create a table to store origin metadata.

CREATE TABLE metadata.t_origin (

    origin_id uuid NOT NULL PRIMARY KEY,
    
    origin_when timestamptz NOT NULL,
    origin_description varchar(1000) NULL,
    origin_reason varchar(1000) NULL,
    origin_source varchar(100) NULL,

    user_id uuid NOT NULL,
    organization_id uuid NOT NULL,

    proxy_agent uuid NULL,  -- User is impersonated by Agent
    proxy_subject uuid NULL -- User acts on behalf of Subject
);

-- Create views for schema, table, and column metadata on base tables.

CREATE VIEW metadata.v_schema AS
SELECT
    n.nspname AS schema_name,
    (
        SELECT
            COUNT(*)
        FROM
            pg_class c
        WHERE
            c.relnamespace = n.oid
    ) AS object_count,
    (
        SELECT
            COUNT(*)
        FROM
            information_schema.tables t
        WHERE
            t.table_schema = n.nspname
            AND t.table_type = 'BASE TABLE'
    ) AS table_count
FROM
    pg_namespace n
WHERE
    n.nspname NOT LIKE 'pg_%'
    AND n.nspname NOT IN ('information_schema');

CREATE VIEW metadata.v_table AS
SELECT
    s.schema_name,
    t.table_name,
    (
        SELECT
            COUNT(*)
        FROM
            information_schema.columns AS c
        WHERE
            c.table_schema = s.schema_name
            AND c.table_name = t.table_name
    ) AS column_count,
    (
        SELECT
            n_live_tup
        FROM
            pg_stat_user_tables AS x
        WHERE
            x.schemaname = s.schema_name
            AND x.relname = t.table_name
    ) AS row_count
FROM
    metadata.v_schema AS s
    JOIN information_schema.tables AS t ON t.table_schema = s.schema_name
WHERE
    t.table_type = 'BASE TABLE';

CREATE VIEW metadata.v_table_column AS
SELECT
    T.schema_name,
    T.table_name,
    C.column_name,
    C.data_type,
    CASE
        WHEN C.is_nullable = 'YES' THEN FALSE
        WHEN C.is_nullable = 'NO' THEN TRUE
        ELSE NULL
    END AS is_required,
    C.character_maximum_length AS maximum_length,
    C.numeric_precision,
    C.numeric_scale,
    C.ordinal_position
FROM
    information_schema.columns C
    JOIN metadata.v_table AS T ON C.TABLE_NAME = T.table_name
    AND C.TABLE_SCHEMA = T.schema_name;

create view metadata.v_primary_key as
select ns.nspname as schema_name ,
       tb.relname as table_name ,
       att.attname as column_name ,
       con.conname as constraint_name ,
       pg_catalog.format_type(att.atttypid, att.atttypmod) as data_type ,
       att.attidentity != '' as is_identity
from pg_constraint as con
join pg_class as tb on tb.oid = con.conrelid
join pg_namespace as ns on ns.oid = tb.relnamespace
join pg_attribute as att on att.attrelid = tb.oid
and att.attnum = any (con.conkey)
where con.contype = 'p'; -- Only include primary key constraints
