
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
