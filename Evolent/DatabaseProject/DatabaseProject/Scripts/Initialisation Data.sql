
INSERT INTO dbo.ApplicationSettings (AppToken, AppName)
VALUES(NEWID(), 'WEB APP')
GO

INSERT INTO dbo.ApplicationSettings (AppToken, AppName)
VALUES(NEWID(), 'MOBILE APP')
GO

INSERT INTO Users (UserName, Password, UserToken)
VALUES('evolent', 'evolent@123', NEWID())
GO
