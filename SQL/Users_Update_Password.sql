USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_Update_Password]    Script Date: 12/31/2022 1:47:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Jonathan S.
-- Create date: 12/9/2022
-- Description: Proc to update a users password.
-- Code Reviewer:

ALTER proc [dbo].[Users_Update_Password]
			@Id int
			,@Password varchar(100)

/*

Declare @Id int = 1
		,@Password varchar(100) = 'updatedPassword1!'

Execute dbo.Users_SelectById 
				@Id

Execute dbo.Users_Update_Password 
				@Id
				,@Password

Execute dbo.Users_SelectById 
				@Id
*/

AS

BEGIN

Declare @DateNow datetime2(7) = getutcdate()

UPDATE [dbo].[Users]
   SET [Password] = @Password
      ,[DateModified] = @DateNow
 WHERE Id = @Id

END


