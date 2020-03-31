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