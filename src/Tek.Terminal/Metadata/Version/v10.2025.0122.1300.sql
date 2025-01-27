CREATE TABLE content.t_translation (

    translation_id uuid NOT NULL PRIMARY KEY,
    translation_text JSONB NOT NULL,
    modified_when timestamptz NOT NULL    
);

CREATE TABLE security.t_partition (

    partition_id uuid NOT NULL PRIMARY KEY,
    
    partition_domains VARCHAR(400) NOT NULL,
    partition_email VARCHAR(254) NOT NULL,
    partition_host VARCHAR(50) NOT NULL,
    partition_name VARCHAR(50) NOT NULL,
    partition_number INT NOT NULL,
    partition_settings JSON NULL,
    partition_slug VARCHAR(3) NOT NULL,
    partition_testers VARCHAR(400) NULL,

    modified_when timestamptz NOT NULL
);

CREATE TABLE security.t_organization (

    organization_id uuid NOT NULL PRIMARY KEY,
    
    organization_name VARCHAR(50) NOT NULL,
    organization_number INT NOT NULL,
    organization_slug VARCHAR(3) NOT NULL,
    
    partition_slug VARCHAR(3) NOT NULL,

    modified_when timestamptz NOT NULL
);

CREATE TABLE security.t_password (

    password_id UUID NOT NULL PRIMARY KEY, -- v3 UUID for email address
    password_email VARCHAR(254) NULL,      -- storing the actual email address is optional

    password_hash VARCHAR(100) NOT NULL,
    password_expiry TIMESTAMPTZ NOT NULL,
    
    default_plaintext VARCHAR(100) NULL,
    default_expiry TIMESTAMPTZ NULL,
    
    created_when TIMESTAMPTZ NOT NULL,
    
    last_forgotten_when TIMESTAMPTZ NULL,
    last_modified_when TIMESTAMPTZ NULL
);

CREATE TABLE metadata.t_origin (

    origin_id uuid NOT NULL PRIMARY KEY,
    
    origin_when timestamptz NOT NULL,
    origin_description varchar(1000) NULL,
    origin_reason varchar(1000) NULL,
    origin_source varchar(100) NULL,

    user_id uuid NOT NULL,
    organization_id uuid NOT NULL,
    proxy_id uuid NULL
);

INSERT INTO security.t_partition 
 (partition_host, partition_email, partition_domains, partition_testers, partition_id, partition_name, partition_number, partition_slug, modified_when) 
values
 ('e01.shiftiq.com', 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '2b476ca4-6719-43b2-a4c9-5f476725d132', 'Demo', 1, 'E01', now())
,('e02.insite.com' , 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'sandra@bcpvpa.bc.ca,david@bcpvpa.bc.ca', 'd88d0913-1c49-48d6-9c0b-2e0aef769d28', 'General', 2, 'E02', now())
,('e03.insite.com' , 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '01eefbf0-9250-4396-842f-698d0bdc7b1a', 'CMDS', 3, 'E03', now())
,('e04.shiftiq.com', 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '19443ea2-b880-4652-bc46-0521b442f2f4', 'Skilled Trades BC', 4, 'E04', now())
,('e05.shiftiq.com', 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', 'c0a5f349-44a9-42e2-a08b-d04b093c099d', 'Demo', 5, 'E05', now())
,('e06.insite.com' , 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '32bb015f-8720-4d04-b480-1421ced0a4ec', 'Inspire Global Assessments', 6, 'E06', now())
,('e07.shiftiq.com', 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '5d4efaf9-0b12-4d4c-9250-cde9809d5531', 'SkillsCheck', 7, 'E07', now())
,('e99.shiftiq.com', 'support@shiftiq.com', 'cmds.app,insite.com,insitedemos.com,insitemail.com,insitemessages.com,insitesystems.com,itabc.ca,itaportal.ca,keyeracmds.com,membertech.com,mg.shiftiq.com,shiftiq.com,skilledtradesbc.ca,skillspassport.com', 'girl.feldy@gmail.com', '6c48f170-9afd-4140-a78f-81fadde0e2ea', 'Test', 99, 'E99', now())
;