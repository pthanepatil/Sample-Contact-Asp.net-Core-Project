CREATE TABLE [dbo].[ApplicationSettings] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [AppToken] NVARCHAR (36) NOT NULL,
    [AppName]  VARCHAR (20)  NOT NULL,
    [isActive] BIT           CONSTRAINT [DF_ApplicationSettings_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_ApplicationSettings_AppToken] PRIMARY KEY CLUSTERED ([AppToken] ASC)
);

