CREATE PROCEDURE sp_validate_token
(
	@appToken NVARCHAR(36),
	@userToken NVARCHAR(36) = NULL,
	@validateBothToken BIT = 1
)
AS
BEGIN

	DECLARE @isValid BIT;

	IF @validateBothToken = 0
	BEGIN
		
		SELECT @isValid = 1 FROM dbo.ApplicationSettings WHERE AppToken = @appToken AND isActive = 1

		SELECT ISNULL(@isValid, 0)

	END
	ELSE
	BEGIN

		SELECT @isValid = 1 
		WHERE EXISTS(SELECT 1 FROM dbo.Users WHERE UserToken = @userToken AND isActive = 1)
		AND EXISTS(SELECT 1 FROM dbo.ApplicationSettings WHERE AppToken = @appToken AND isActive = 1)

		SELECT ISNULL(@isValid, 0)

	END
END