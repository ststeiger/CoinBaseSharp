
-- DELETE FROM t_map_currency_rate
SELECT 
	 cur_uid
	,cur_time
	,cur_name
	,cur_rate
FROM t_map_currency_rate
ORDER BY cur_name 




SELECT 
	 t_map_currency_rate.cur_uid
	,t_map_currency_rate.cur_time
	,t_map_currency_rate.cur_name
	,t_map_currency_rate.cur_rate
	 
	,t_currency.ccy_uid
	,t_currency.ccy_number
	,t_currency.ccy_name
	,t_currency.ccy_abbreviation
	,t_currency.ccy_country_name
	,t_currency.ccy_minor_units
	--,CAST(t_currency.ccy_minor_units as int) 
FROM t_map_currency_rate

LEFT JOIN t_currency 
	ON t_currency.ccy_abbreviation = t_map_currency_rate.cur_name 
	
-- WHERE ccy_uid IS NULL 
--AND cur_name NOT IN (N'JEP', N'MTL', N'GGP', N'IMP', N'LVL', N'BTC', N'LTL', N'EEK', N'ZMK')

ORDER BY cur_name 
;



SELECT 
	 COUNT(cur_uid) AS CC 
	,COUNT(cur_rate) AS RC 
	,COUNT(DISTINCT cur_rate) AS DC 
	,cur_name
	,MAX(cur_rate) AS cur_rate 
FROM t_map_currency_rate
WHERE (1=1) 
AND cur_time = '2016-05-06T07:00:01.000' 
GROUP BY cur_name
ORDER BY DC DESC 
;
