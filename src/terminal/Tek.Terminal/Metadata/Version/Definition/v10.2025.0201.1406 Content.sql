CREATE TABLE content.t_translation (

    translation_id UUID NOT NULL PRIMARY KEY,
    translation_text JSON NOT NULL,

    modified_when TIMESTAMPTZ NOT NULL    
);

CREATE TABLE obsolete.t_translation (

    translation_id UUID NOT NULL PRIMARY KEY,
    
    en TEXT NULL,
	ar TEXT NULL,
	de TEXT NULL,
	eo TEXT NULL,
	es TEXT NULL,
	fr TEXT NULL,
	he TEXT NULL,
	it TEXT NULL,
	ja TEXT NULL,
	ko TEXT NULL,
	la TEXT NULL,
	nl TEXT NULL,
	no TEXT NULL,
	pa TEXT NULL,
	pl TEXT NULL,
	pt TEXT NULL,
	ru TEXT NULL,
	sv TEXT NULL,
	uk TEXT NULL,
	zh TEXT NULL,

    modified_when TIMESTAMPTZ NOT NULL DEFAULT now()
);