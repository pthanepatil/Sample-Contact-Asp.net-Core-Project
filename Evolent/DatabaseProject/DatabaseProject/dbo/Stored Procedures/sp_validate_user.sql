CREATE PROCEDURE sp_validate_user
(
	@userName NVARCHAR(20),
	@password NVARCHAR(20)
)
AS
BEGIN

	SELECT *
	FROM dbo.Users (NOLOCK)
	WHERE UserName = @userName AND Password = @password

END