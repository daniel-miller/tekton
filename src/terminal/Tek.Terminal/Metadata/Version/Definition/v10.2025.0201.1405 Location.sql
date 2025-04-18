CREATE TABLE location.t_country (

    country_id UUID NOT NULL PRIMARY KEY,

    country_code VARCHAR(2) NOT NULL,
    country_name VARCHAR(50) NOT NULL,
    
    languages VARCHAR(60) NULL,
    
    currency_code VARCHAR(3) NULL,
    currency_name VARCHAR(20) NULL,
    
    top_level_domain VARCHAR(3) NULL,
    
    continent_code VARCHAR(2) NULL,
    
    capital_city_name VARCHAR(60) NULL
);

CREATE TABLE location.t_province (

    province_id UUID NOT NULL PRIMARY KEY,

    province_code VARCHAR(2) NULL,
    province_name VARCHAR(80) NOT NULL,
    province_name_translation VARCHAR(200) NULL,

    country_code VARCHAR(2) NOT NULL,
    country_id UUID NULL
);