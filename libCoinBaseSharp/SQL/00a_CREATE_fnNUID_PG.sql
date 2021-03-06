-- Function: nuid()

-- DROP FUNCTION IF EXISTS dbo.nuid();

CREATE SCHEMA IF NOT EXISTS dbo;


CREATE OR REPLACE FUNCTION dbo.nuid()
  RETURNS uuid AS 
$BODY$ 
BEGIN
    RETURN CAST('00000000-0000-0000-0000-000000000000' AS uuid); 
END
$BODY$ 
LANGUAGE plpgsql VOLATILE
;
