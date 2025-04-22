CREATE TABLE timeline.t_aggregate (

    aggregate_id UUID NOT NULL PRIMARY KEY,

    aggregate_type VARCHAR(100) NOT NULL,
    aggregate_root UUID NOT NULL
);

CREATE TABLE timeline.t_event (

    event_id UUID NOT NULL PRIMARY KEY,
    
    event_type VARCHAR(100) NOT NULL,
    event_data TEXT NOT NULL,

    aggregate_id UUID NOT NULL,
    aggregate_version INT NOT NULL,

    origin_id UUID NOT NULL
);