
;WITH CTE_Rate AS (
SELECT 
	(
		SELECT TOP 1 cur_rate 
		FROM t_map_currency_rate
		WHERE cur_name = 'EUR'
		AND cur_time = '2016-05-06T07:00:01.000'
	) AS EUR_From_USD
	,
	(
		SELECT TOP 1 cur_rate 
		FROM t_map_currency_rate
		WHERE cur_name = 'CHF'
		AND cur_time = '2016-05-06T07:00:01.000'	
	) AS CHF_From_USD 
) 
SELECT * 
	,CHF_From_USD/NULLIF(EUR_From_USD, 0.0) AS From_EUR_to_CHF 
	,EUR_From_USD/NULLIF(CHF_From_USD, 0.0) AS From_CHF_to_EUR 
FROM CTE_Rate 
;


;WITH CTE_RateMatrix AS ( 
	SELECT 
		 Rate_CCY1.cur_name AS Name1 
		,Rate_CCY1.cur_rate AS Rate1 
		,Rate_CCY2.cur_name AS Name2 
		,Rate_CCY2.cur_rate AS Rate2 
	FROM 
	( 
		SELECT 
			 cur_name 
			,MAX(cur_rate) AS cur_rate 
		FROM t_map_currency_rate 
		WHERE (1=1) 
		AND cur_time = '2016-05-06T07:00:01.000' 
		GROUP BY cur_name 
	) AS Rate_CCY1 
	
	CROSS JOIN 
	( 
		SELECT 
			 cur_name 
			,MAX(cur_rate) AS cur_rate 
		FROM t_map_currency_rate 
		WHERE (1=1) 
		AND cur_time = '2016-05-06T07:00:01.000' 
		GROUP BY cur_name 
	) AS Rate_CCY2 
) 
SELECT 
	 CTE_RateMatrix.* 
	,Rate1/NULLIF(Rate2, 0.0) AS From_Name2_to_Name1 
	,Rate2/NULLIF(Rate1, 0.0) AS From_Name1_to_Name2 
	,t_currency.ccy_name 
	,t_currency.ccy_country_name 
	,t_currency.ccy_name_normalized 
FROM CTE_RateMatrix 

LEFT JOIN t_currency 
	ON t_currency.ccy_abbreviation = CTE_RateMatrix.Name2 

WHERE (1=1) 
-- AND Name1 = 'CHF' 
-- AND Name2 = 'EUR' 
-- AND Name1 IN ('EUR', 'USD', 'CHF', 'GBP') 
-- AND Name2 IN ('EUR', 'USD', 'CHF', 'GBP') 
AND Name1 = 'CHF' 
--AND Name2 = 'RUB' 
--AND Name2 IN ('RUB', 'UAH', 'MAD', 'USD') 


ORDER BY From_Name1_to_Name2 DESC 
--ORDER BY Name1, Name2 
