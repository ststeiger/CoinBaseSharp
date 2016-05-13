
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[t_currency]') AND type in (N'U'))
EXECUTE('
CREATE TABLE dbo.t_currency 
( 
	 ccy_uid uniqueidentifier NOT NULL 
	,ccy_number int NULL 
	,ccy_name national character varying(255) NULL 
	,ccy_abbreviation national character varying(20) NULL 
	,ccy_country_name national character varying(255) NULL 
	,ccy_name_normalized national character varying(255) NULL
	,ccy_minor_units national character varying(20) NULL 
	,CONSTRAINT PK_t_currency PRIMARY KEY (ccy_uid) 
);
');


-- DELETE FROM t_currency; 
-- UPDATE t_currency SET ccy_minor_units = NULL WHERE ccy_minor_units = 'N.A.' 

-- ALTER TABLE dbo.t_currency ALTER COLUMN ccy_minor_units int NULL

CREATE TABLE IF NOT EXISTS t_currency 
( 
	 ccy_uid uuid NOT NULL 
	,ccy_number int NULL 
	,ccy_name national character varying(255) NULL 
	,ccy_abbreviation national character varying(20) NULL 
	,ccy_country_name national character varying(255) NULL 
	,ccy_name_normalized national character varying(255) NULL 
	,ccy_minor_units national character varying(20) NULL 
	,CONSTRAINT PK_t_currency PRIMARY KEY (ccy_uid) 
);

-- ALTER TABLE t_currency ALTER COLUMN ccy_minor_units type int using (ccy_minor_units::int)  
