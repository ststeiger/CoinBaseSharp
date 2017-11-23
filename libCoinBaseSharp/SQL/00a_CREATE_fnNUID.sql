
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NUID]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION dbo.NUID 
GO




CREATE FUNCTION dbo.NUID() 
	RETURNS uniqueidentifier 
AS
BEGIN
	RETURN CAST('00000000-0000-0000-0000-000000000000' AS uniqueidentifier) 
END


GO

