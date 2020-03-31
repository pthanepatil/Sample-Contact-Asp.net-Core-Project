
-----------Application Config-------------------
CREATE TABLE dbo.ApplicationSettings
(
	Id INT IDENTITY(1, 1),
	AppToken NVARCHAR(36) CONSTRAINT PK_ApplicationSettings_AppToken PRIMARY KEY,
	AppName VARCHAR(20) NOT NULL,
	isActive BIT CONSTRAINT DF_ApplicationSettings_isActive DEFAULT(1)
)
GO

INSERT INTO dbo.ApplicationSettings (AppToken, AppName)
VALUES(NEWID(), 'WEB APP')

INSERT INTO dbo.ApplicationSettings (AppToken, AppName)
VALUES(NEWID(), 'MOBILE APP')
GO

SELECT * FROM dbo.ApplicationSettings


-----------------Login User-------------------------
CREATE TABLE dbo.Users
(
	Id INT IDENTITY(1, 1) CONSTRAINT PK_Users_Id PRIMARY KEY,
	UserName VARCHAR(20) CONSTRAINT UK_Users_UserName UNIQUE NOT NULL,
	Password VARCHAR(20) NOT NULL,
	UserToken NVARCHAR(36) CONSTRAINT UK_Users_UserToken UNIQUE NOT NULL,
	isActive BIT CONSTRAINT DF_Users_isActive DEFAULT(1)
)
GO

INSERT INTO Users (UserName, Password, UserToken)
VALUES('evolent', 'evolent@123', NEWID())
GO

-------------User Contact---------------------------
CREATE TABLE dbo.UserContact
(
	Id INT IDENTITY(1, 1),
	UserId INT, 
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50),
	Email NVARCHAR(50),
	PhoneNumber VARCHAR(15),
	isActive BIT CONSTRAINT DF_UserContact_isActive DEFAULT(1)
	CONSTRAINT FK_UserContact_UserId FOREIGN KEY(UserId) REFERENCES Users(Id),
	CONSTRAINT PK_UserContact_Id PRIMARY KEY (Id)
)