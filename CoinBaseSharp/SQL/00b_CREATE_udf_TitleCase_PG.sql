
CREATE SCHEMA IF NOT EXISTS dbo;

-- DROP FUNCTION IF EXISTS dbo.udf_TitleCase(varchar(4000));

CREATE OR REPLACE FUNCTION dbo.udf_TitleCase(__inputString varchar(4000))
  RETURNS varchar(4000) AS 
$BODY$ 
BEGIN
    RETURN initcap(__inputString);
END
$BODY$ 
LANGUAGE plpgsql VOLATILE
;

-- Warning: initcap: bug on accent characters...

/*
;with cte as (
SELECT *, dbo.udf_TitleCase(REPLACE(ccy_country_name, ' (THE)', '')) as udf FROM t_currency 
)
select * from cte 
where ccy_name_normalized <> udf 
*/
