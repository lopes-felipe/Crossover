USE [Crossover]
GO

/*---------------------------------------------------------------- application_session ----------------------------------------------------------------*/
IF OBJECT_ID(N'application_session', N'U') IS NOT NULL
    DROP TABLE [application_session]
GO

CREATE TABLE [application_session](
    session_id     bigint			NOT NULL IDENTITY(1, 1) PRIMARY KEY,
    application_id varchar(32)		NOT NULL REFERENCES [application](application_id) ON UPDATE CASCADE ON DELETE CASCADE,
	access_token   uniqueidentifier NOT NULL,
	active		   bit				NOT NULL,
	created_date   datetime			NOT NULL,
	valid_until    datetime			NOT NULL)
GO

CREATE NONCLUSTERED INDEX [IX_application_session_access_token] ON [dbo].[application_session]
(
	[access_token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [IX_application_session_application_id_active] ON [dbo].[application_session]
(
	[application_id] ASC,
	[active] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/*-----------------------------------------------------------------------------------------------------------------------------------------------------*/

/*-------------------------------------------------------------------- application --------------------------------------------------------------------*/
ALTER TABLE [application] ADD
	[restricted_access_until] datetime NULL
GO
/*-----------------------------------------------------------------------------------------------------------------------------------------------------*/

/*--------------------------------------------------------------- session_configuration ---------------------------------------------------------------*/
IF OBJECT_ID(N'session_configuration', N'U') IS NOT NULL
    DROP TABLE [session_configuration]
GO

CREATE TABLE [session_configuration](
    configuration_id		  int NOT NULL PRIMARY KEY,
	session_lifetime_minutes  int NOT NULL)
GO

INSERT INTO [session_configuration] 
			(configuration_id, session_lifetime_minutes)
	 VALUES (1, 60)
GO
/*-----------------------------------------------------------------------------------------------------------------------------------------------------*/

/*----------------------------------------------------------------- application_call ------------------------------------------------------------------*/
IF OBJECT_ID(N'application_call', N'U') IS NOT NULL
    DROP TABLE [application_call]
GO

CREATE TABLE [application_call](
	call_id		   bigint	   NOT NULL PRIMARY KEY IDENTITY(1,1),
    application_id varchar(32) NOT NULL REFERENCES [application](application_id) ON UPDATE CASCADE ON DELETE CASCADE,
	call_date	   datetime	   NOT NULL)
GO
/*-----------------------------------------------------------------------------------------------------------------------------------------------------*/