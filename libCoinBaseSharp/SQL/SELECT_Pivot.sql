

/*
-- DELETE FROM t_map_currency_rate
SELECT 
	 cur_name
	,@sql
	,MAX([2016-05-06T07:00:01.000]) AS [2016-05-06T07:00:01.000] 
	,MAX([2666-06-06T06:06:06.666]) AS [2666-06-06T06:06:06.666] 
FROM t_map_currency_rate 
PIVOT
(
	MAX(cur_rate)
	FOR cur_time IN
	(
		  [2016-05-06T07:00:01.000] 
		 ,[2666-06-06T06:06:06.666] 
	) 
) AS pvt 

GROUP BY 
	 cur_name 
	 
ORDER BY cur_name 
*/




DECLARE @sqlAgg nvarchar(MAX) 
DECLARE @sqlCmd nvarchar(MAX) 


SELECT 
	@sqlAgg = COALESCE(@sqlAgg + N', ', N'') + 
  'MAX(' 
  + QUOTENAME(REPLACE(CONVERT(nvarchar(23), cur_time, 121), N' ', N'T')) 
  + ') AS ' 
  + QUOTENAME(REPLACE(CONVERT(nvarchar(23), cur_time, 121), N' ', N'T')) 
FROM 
( 
	SELECT DISTINCT TOP 999999999 cur_time 
	FROM t_map_currency_rate 
	GROUP BY cur_time 
	ORDER BY cur_time DESC 
) AS t 



SELECT 
	@sqlCmd = COALESCE(@sqlCmd + N', ', N'') + 
		QUOTENAME(REPLACE(CONVERT(nvarchar(23), cur_time, 121), N' ', N'T')) 
FROM 
( 
	SELECT TOP 999999999 cur_time 
	FROM t_map_currency_rate 
	GROUP BY cur_time 
	ORDER BY cur_time DESC 
) AS t 



SET @sqlCmd = '
SELECT 
	 cur_name
	,' + @sqlAgg + N'
FROM t_map_currency_rate 
PIVOT
(
	MAX(cur_rate)
	FOR cur_time IN
	( 
		' + @sqlCmd + ' 
	) 
) AS pvt 

GROUP BY 
	 cur_name 
	 
ORDER BY cur_name 
'

-- PRINT @sqlCmd 
EXECUTE(@sqlCmd) 
