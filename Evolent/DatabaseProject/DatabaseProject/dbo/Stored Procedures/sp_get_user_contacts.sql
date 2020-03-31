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