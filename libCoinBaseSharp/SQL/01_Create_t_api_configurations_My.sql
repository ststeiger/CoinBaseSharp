

CREATE TABLE IF NOT EXISTS t_api_configurations
(
	 api_uid character varying(36) NOT NULL 
	,api_name national character varying(300) NULL 
	,api_app_id national character varying(300) NULL 
	,api_key_public national character varying(300) NULL 
	,api_key_secret national character varying(300) NULL 
	,api_href national character varying(4000) NULL 
	,api_comment national character varying(4000) NULL 
	,CONSTRAINT PK_t_api_configurations PRIMARY KEY(api_uid) 
);
