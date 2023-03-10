USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Select_ByTokenType]    Script Date: 12/31/2022 1:47:19 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Proc to Select a UserToken using Token type.
-- Code Reviewer:

ALTER proc [dbo].[UserTokens_Select_ByTokenType]
			@TokenType int

/*
Declare @TokenType int = 1

Execute dbo.UserTokens_Select_ByTokenType
		@TokenType
*/

AS

BEGIN

SELECT [Token]
      ,[UserId]
      ,[TokenType]
  FROM [dbo].[UserTokens]
  WHERE TokenType = @TokenType

END


