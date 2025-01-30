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