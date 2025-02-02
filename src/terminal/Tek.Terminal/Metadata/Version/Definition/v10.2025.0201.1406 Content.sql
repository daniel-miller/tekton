CREATE TABLE content.t_translation (

    translation_id UUID NOT NULL PRIMARY KEY,
    translation_text JSONB NOT NULL,

    modified_when TIMESTAMPTZ NOT NULL    
);