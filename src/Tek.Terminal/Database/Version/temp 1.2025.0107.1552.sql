drop table if exists release.TVersion;

create table release.TVersion (
    VersionNumber varchar(30) not null primary key,
    ScriptContent varchar(max) not null,
    ScriptExecuted datetimeoffset not null
);