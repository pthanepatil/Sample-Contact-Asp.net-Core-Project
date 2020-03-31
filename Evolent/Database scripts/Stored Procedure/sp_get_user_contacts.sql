CREATE PROCEDURE sp_get_user_contacts
(
	@userId INT,
	@contactId INT = NULL
)
AS
BEGIN

	SELECT Id AS ContactId, FirstName, LastName, Email, PhoneNumber, isActive
	FROM dbo.UserContact (NOLOCK)
	WHERE UserId = @userId AND (@contactId IS NULL OR Id = @contactId)

END


-------------------------Add/Update UserContact-------------------------
CREATE PROCEDURE sp_save_user_contact
(
	@userId INT,
	@contactId INT,
	@firstName NVARCHAR(50),
	@lastName NVARCHAR(50),
	@phoneNumber VARCHAR(15),
	@email NVARCHAR(50),
	@isActive BIT
)
AS
BEGIN
	SET NOCOUNT OFF;

BEGIN TRAN
BEGIN TRY

	IF EXISTS(SELECT 1 FROM dbo.UserContact (NOLOCK) WHERE UserId = @userId AND Id = @contactId)
	BEGIN
		UPDATE dbo.UserContact
		SET FirstName = @firstName,
			LastName = @lastName,
			Email = @email,
			PhoneNumber = @phoneNumber,
			isActive = @isActive
		WHERE UserId = @userId AND Id = @contactId
	END
	ELSE
	BEGIN
		INSERT INTO dbo.UserContact (UserId, FirstName, LastName, Email, PhoneNumber, isActive)
		VALUES (@userId, @firstName, @lastName, @email, @phoneNumber, @isActive)
	END

	SELECT @@TRANCOUNT;

COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN;

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
  
    SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage, -- Message text.  
               @ErrorSeverity, -- Severity.  
               @ErrorState -- State.  
               ); 
END CATCH
END



-------------------------Update UserContact Status-------------------------
CREATE PROCEDURE sp_update_contact_status
(
	@userId INT,
	@contactId INT,
	@isActive BIT
)
AS
BEGIN
	SET NOCOUNT OFF;

BEGIN TRAN
BEGIN TRY

	UPDATE dbo.UserContact
	SET isActive = @isActive
	WHERE UserId = @userId AND Id = @contactId
	
	SELECT @@TRANCOUNT;

COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN;

	DECLARE @ErrorMessage NVARCHAR(4000);  
    DECLARE @ErrorSeverity INT;  
    DECLARE @ErrorState INT;  
  
    SELECT   
        @ErrorMessage = ERROR_MESSAGE(),  
        @ErrorSeverity = ERROR_SEVERITY(),  
        @ErrorState = ERROR_STATE();  
   
    RAISERROR (@ErrorMessage,
               @ErrorSeverity,
               @ErrorState
               ); 
END CATCH
END