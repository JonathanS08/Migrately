USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Delete_ByToken]    Script Date: 12/31/2022 1:47:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Proc to delete a userToken by passing a token.
-- Code Reviewer: Sorin W.

-- MODIFIED BY: Jonathan S.
-- MODIFIED DATE:12/9/2022
-- Code Reviewer:
-- Note: added userId to doubly enusre correct deletion.

ALTER proc [dbo].[UserTokens_Delete_ByToken]
			@Token varchar(200)
			,@Id int
/*
Declare
	@Token varchar(200) = '022e8468-7cec-44aa-adcf-9be17b99eae0'
	,@Id int = 52

Execute dbo.UserTokens_Delete_ByToken
		@Token
		,@Id
*/

AS

BEGIN

DELETE FROM [dbo].[UserTokens]
      WHERE Token = @Token AND UserId = @Id
END


