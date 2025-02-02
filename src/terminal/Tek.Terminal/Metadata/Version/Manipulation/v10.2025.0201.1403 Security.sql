-- Load partitions 1-7 and 99.

INSERT INTO security.t_partition 
 (partition_number, partition_slug, partition_host, partition_name, partition_email, modified_when, partition_testers) 
VALUES
 ( 1, 'E01', 'e01.shiftiq.com', 'Demo'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 2, 'E02', 'e02.insite.com' , 'General'          , 'support@shiftiq.com', now(), 'sandra@bcpvpa.bc.ca,david@bcpvpa.bc.ca')
,( 3, 'E03', 'e03.insite.com' , 'CMDS'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 4, 'E04', 'e04.shiftiq.com', 'Skilled Trades BC', 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 5, 'E05', 'e05.shiftiq.com', 'Demo'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 6, 'E06', 'e06.insite.com' , 'Inspire'          , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,( 7, 'E07', 'e07.shiftiq.com', 'SkillsCheck'      , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
,(99, 'E99', 'e99.shiftiq.com', 'Test'             , 'support@shiftiq.com', now(), 'girl.feldy@gmail.com')
;