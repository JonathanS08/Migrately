USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectPass_ByEmail]    Script Date: 12/31/2022 1:47:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Select a password of a user by using email to find them.
-- Code Reviewer: Sorin W.

ALTER proc [dbo].[Users_SelectPass_ByEmail]
			@Email nvarchar(255)

/*
Declare @Email nvarchar(255) = 'sample@email.com'

Execute dbo.Users_SelectPass_ByEmail
		@Email
*/

AS

BEGIN

SELECT 
      [Password]
      
  FROM [dbo].[Users]
  WHERE Email = @Email

END


