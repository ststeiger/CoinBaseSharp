
SELECT 
	 t_currency.ccy_uid
	,t_currency.ccy_number
	,t_currency.ccy_name
	,t_currency.ccy_abbreviation
	,t_currency.ccy_country_name
	,t_currency.ccy_minor_units
	--,CAST(t_currency.ccy_minor_units as int) 
FROM t_currency
-- WHERE t_currency.ccy_minor_units > 2 
--WHERE t_currency.ccy_number = 674
-- ORDER BY ccy_abbreviation 
ORDER BY ccy_number
  
  
