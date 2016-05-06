
SELECT 
     extractedRow.XmlCol.value('local-name(.)', 'nvarchar(255)') AS ColumnName 
	,extractedRow.XmlCol.value('.', 'nvarchar(max)') AS ColumnValue
FROM 
(
	SELECT * 
	FROM t_map_currency_rate 
	WHERE cur_uid = 'E620572A-6C78-4750-A9B6-BFA6D6680C05' 
    FOR XML PATH, TYPE 
) AS fromTable(asXmlColumn) 

CROSS APPLY fromTable.asXmlColumn.nodes('row/*') AS extractedRow(XmlCol)
