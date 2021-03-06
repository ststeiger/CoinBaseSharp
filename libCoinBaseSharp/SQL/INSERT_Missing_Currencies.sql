
WITH CTE AS (
			  SELECT N'JEP' AS abbv, N'Jersey Pound' AS Name, N'Jersey' AS country 
			   -- Jersey is in currency union with the United Kingdom, and the Jersey pound is not a separate currency[citation needed] 
			   -- but is an issue of banknotes and coins by the States of Jersey denominated in pound sterling, 
			   -- in a similar way to the banknotes issued in Scotland and Northern Ireland (see Banknotes of the pound sterling)
			  
	UNION ALL SELECT N'MTL', N'Maltese lira', N'Malta' -- † 31 December 2007.
	UNION ALL SELECT N'GGP', N'Guernsey Pound', N'Guernsey' -- unclear
	UNION ALL SELECT N'IMP', N'Isle of Man Pound', N'Isle of Man' -- Manx pound (Manx: Punt Manninagh) is the currency of the Isle of Man, in parity with the pound sterling. The Manx pound is divided into 100 pence
	UNION ALL SELECT N'LVL', N'Latvian Lats', N'Latvia' -- † 1 January 2014
	UNION ALL SELECT N'BTC', N'Bitcoin', 'International' -- not ISO
	UNION ALL SELECT N'LTL', N'Lithuanian Litas', 'Lithuania' -- † 1 January 2015
	UNION ALL SELECT N'EEK', N'Estonian Kroon', 'Estonia' --  † 14 January 2011
	UNION ALL SELECT N'ZMK', N'Zambian Kwacha', 'Zambia' -- Alive
)
INSERT INTO t_currency( ccy_uid, ccy_number, ccy_name, ccy_abbreviation, ccy_country_name, ccy_minor_units )
SELECT 
	 '0000000' + CAST(-1 + ROW_NUMBER()  OVER( ORDER BY abbv) AS varchar(20))  + '-4666-0000-0000-000000000000' AS ccy_uid -- uniqueidentifier 
	,CASE WHEN ROW_NUMBER() OVER( ORDER BY abbv) = 1 
		THEN 666
		ELSE 6660 + ROW_NUMBER() OVER( ORDER BY abbv) 
	END AS ccy_number -- int 
	
	,Name AS ccy_name -- nvarchar(255)
	,abbv AS ccy_abbreviation -- nvarchar(20)
	,country AS ccy_country_name -- nvarchar(255)
	,2 AS ccy_minor_units -- int
FROM CTE 
;
