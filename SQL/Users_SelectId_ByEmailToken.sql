USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectId_ByEmailToken]    Script Date: 12/31/2022 1:47:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Jonathan S.
-- Create date: 11/29/2022
-- Description: Select an Id of a user by using email and token to find them.
-- Code Reviewer:


ALTER proc [dbo].[Users_SelectId_ByEmailToken]
			@Email nvarchar(255)
			,@Token varchar(200)
/*

DECLARE 
	@Email nvarchar(255) = 'Juan1@dispostable.com'
	,@Token varchar(200) = '92fbaa5a-cfdb-4535-a5d0-5551b737b7ab'

EXECUTE dbo.Users_SelectEmailToken_ById
		@Email
		,@Token
*/

AS

BEGIN

SELECT u.[Id]
      ,u.[Email]
	  ,ut.[Token]
      
  FROM [dbo].[Users] as u inner join dbo.UserTokens as ut
			on u.id = ut.UserId
  WHERE u.Email = @Email AND ut.Token = @Token

END


