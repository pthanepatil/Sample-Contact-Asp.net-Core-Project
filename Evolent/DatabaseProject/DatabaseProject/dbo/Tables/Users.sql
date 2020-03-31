CREATE TABLE [dbo].[Users] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [UserName]  VARCHAR (20)  NOT NULL,
    [Password]  VARCHAR (20)  NOT NULL,
    [UserToken] NVARCHAR (36) NOT NULL,
    [isActive]  BIT           CONSTRAINT [DF_Users_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [UK_Users_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UK_Users_UserName] UNIQUE NONCLUSTERED ([UserName] ASC),
    CONSTRAINT [UK_Users_UserToken] UNIQUE NONCLUSTERED ([UserToken] ASC)
);

