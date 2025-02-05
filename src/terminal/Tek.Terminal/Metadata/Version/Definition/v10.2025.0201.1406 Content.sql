CREATE TABLE content.t_translation (

    translation_id UUID NOT NULL PRIMARY KEY,
    translation_text JSONB NOT NULL,

    modified_when TIMESTAMPTZ NOT NULL    
);

-- This models the old original translation table in Microsoft SQL Server.

CREATE TABLE content.t_translation_legacy (
    text_en TEXT NULL,
    text_ar TEXT NULL,
    text_de TEXT NULL,
    text_eo TEXT NULL,
    text_es TEXT NULL,
    text_fr TEXT NULL,
    text_he TEXT NULL,
    text_it TEXT NULL,
    text_ja TEXT NULL,
    text_ko TEXT NULL,
    text_la TEXT NULL,
    text_nl TEXT NULL,
    text_no TEXT NULL,
    text_pa TEXT NULL,
    text_pl TEXT NULL,
    text_pt TEXT NULL,
    text_ru TEXT NULL,
    text_sv TEXT NULL,
    text_uk TEXT NULL,
    text_zh TEXT NULL,
    created_when TIMESTAMPTZ NOT NULL,
    modified_when TIMESTAMPTZ NOT NULL,
    translation_id UUID NOT NULL PRIMARY KEY
)