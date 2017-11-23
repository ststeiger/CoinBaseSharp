
ALTER TABLE dbo.t_currency ADD ccy_name_normalized nvarchar(255) NULL;
UPDATE t_currency 
	SET  ccy_country_name = UPPER(ccy_country_name) 
		,ccy_name_normalized = dbo.udf_TitleCase(ccy_country_name) -- MS
		-- ,ccy_name_normalized = InitCap(ccy_country_name) -- PostGre
		



UPDATE t_currency SET ccy_name_normalized = REPLACE(ccy_name_normalized, '(The)', '') 