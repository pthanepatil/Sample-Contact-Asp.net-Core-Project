CREATE TABLE [dbo].[UserContact] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [UserId]      INT           NULL,
    [FirstName]   NVARCHAR (50) NOT NULL,
    [LastName]    NVARCHAR (50) NULL,
    [Email]       NVARCHAR (50) NULL,
    [PhoneNumber] VARCHAR (15)  NULL,
    [isActive]    BIT           CONSTRAINT [DF_UserContact_isActive] DEFAULT ((1)) NULL,
    CONSTRAINT [PK_UserContact_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserContact_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);

