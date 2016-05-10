
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udf_TitleCase]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION dbo.udf_TitleCase 
GO




-- PG: initcap
CREATE FUNCTION dbo.udf_TitleCase(@InputString varchar(4000) ) 
	RETURNS varchar(4000) 
AS
BEGIN
	-- Credits:
	-- http://blog.sqlauthority.com/2007/02/01/sql-server-udf-function-to-convert-text-string-to-title-case-proper-case/
	
	DECLARE @Index int
	DECLARE @Char char(1)
	DECLARE @OutputString varchar(255)
	SET @OutputString = LOWER(@InputString)
	SET @Index = 2
	SET @OutputString =
	STUFF(@OutputString, 1, 1,UPPER(SUBSTRING(@InputString,1,1)))

	WHILE @Index <= LEN(@InputString)
	BEGIN
		SET @Char = SUBSTRING(@InputString, @Index, 1)
		IF @Char IN (' ', ';', ':', '!', '?', ',', '.', '_', '-', '/', '&','''','(', '[')
		IF @Index + 1 <= LEN(@InputString)
			BEGIN
				IF @Char != '''' OR UPPER(SUBSTRING(@InputString, @Index + 1, 1)) != 'S'
					SET @OutputString = STUFF(@OutputString, @Index + 1, 1,UPPER(SUBSTRING(@InputString, @Index + 1, 1)))
			END

		SET @Index = @Index + 1
	END
	
	RETURN ISNULL(@OutputString,'')
END


GO

