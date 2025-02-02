INSERT INTO security.t_resource ( resource_id, resource_type, resource_name )
VALUES ( gen_random_uuid(), 'Test', 'Test Resource' );

INSERT INTO security.t_role ( role_id, role_type, role_name )
VALUES ( gen_random_uuid(), 'Test', 'Test Role' );