

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.t_api_configurations') AND type in (N'U'))
EXECUTE('
CREATE TABLE dbo.t_api_configurations
(
	 api_uid uniqueidentifier NOT NULL 
	,api_name nvarchar(300) NULL 
	,api_app_id nvarchar(300) NULL 
	,api_href nvarchar(4000) NULL 
	,api_comment nvarchar(4000) NULL 
	,CONSTRAINT PK_t_api_configurations PRIMARY KEY(api_uid) 
);
')
