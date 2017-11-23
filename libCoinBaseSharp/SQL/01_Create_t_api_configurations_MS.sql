
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[t_api_configurations]') AND type in (N'U'))
	--DROP TABLE IF EXISTS [dbo].[t_api_configurations]
	DROP TABLE [dbo].[t_api_configurations]
GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.t_api_configurations') AND type in (N'U'))
EXECUTE('
CREATE TABLE dbo.t_api_configurations
(
	 api_uid uniqueidentifier NOT NULL 
	,api_name national character varying(300) NULL 
	,api_app_id national character varying(300) NULL 
	,api_key_public national character varying(300) NULL 
	,api_key_secret national character varying(300) NULL 
	,api_href national character varying(4000) NULL 
	,api_comment national character varying(4000) NULL 
	,CONSTRAINT PK_t_api_configurations PRIMARY KEY(api_uid) 
);
')
