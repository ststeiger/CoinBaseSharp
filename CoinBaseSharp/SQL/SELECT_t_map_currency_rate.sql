
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[t_map_currency_rate]') AND type in (N'U'))
EXECUTE('
CREATE TABLE dbo.t_map_currency_rate
(
	cur_uid uniqueidentifier NULL,
	cur_time datetime NULL,
	cur_name char(3) NULL,
	cur_rate decimal(35, 15) NULL
);
')

-- DELETE FROM t_map_currency_rate
SELECT 
	 cur_uid
	,cur_time
	,cur_name
	,cur_rate
FROM t_map_currency_rate
ORDER BY cur_name 



CREATE TABLE IF NOT EXISTS t_map_currency_rate
(
	cur_uid uuid NULL,
	cur_time timestamp without time zone NULL,
	cur_name char(3) NULL,
	cur_rate decimal(35, 15) NULL
);
