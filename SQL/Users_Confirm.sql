USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_Confirm]    Script Date: 12/31/2022 1:45:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Update a Confirm  of a user by using Id to find them.
-- Code Reviewer: Sorin W.

ALTER proc [dbo].[Users_Confirm]
			@Id int
			,@IsConfirmed bit 

/*
Declare 
	@Id int = 1
	,@IsConfirmed bit = 0

	Execute dbo.Users_SelectById
			@Id
	Execute dbo.Users_Confirm
			@Id
			,@IsConfirmed
	Execute dbo.Users_SelectById
			@Id
*/

AS

BEGIN

Declare @DateNow datetime2 = getutcdate()

UPDATE [dbo].[Users]
   SET 
      [IsConfirmed] = @IsConfirmed
      ,[DateModified] = @DateNow
 WHERE Id = @Id

END


