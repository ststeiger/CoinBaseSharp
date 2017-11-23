
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.price_history') AND type in (N'U'))
EXECUTE('
CREATE TABLE dbo.price_history
(
	uid uniqueidentifier NULL,
	time datetime NULL,
	price decimal(20, 9) NULL
);
')
