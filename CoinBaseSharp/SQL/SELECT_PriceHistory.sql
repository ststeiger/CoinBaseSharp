
-- DELETE FROM price_history

SELECT 
	 CONVERT(char(10), time, 104) 
	,price 
FROM price_history
ORDER BY time ASC 
