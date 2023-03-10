USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[UserTokens_Insert]    Script Date: 12/31/2022 1:47:17 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Proc to insert a User Token .
-- Code Reviewer: Sorin W.

ALTER proc [dbo].[UserTokens_Insert]
			@Token varchar(200)
			,@UserId int
			,@TokenType int
/*
Declare
			@Token varchar(200)= NEWID()
			,@UserId int = 1
			,@TokenType int = 1

Execute dbo.UserTokens_Insert
			@Token
           ,@UserId
           ,@TokenType

Execute dbo.UserTokens_Select_ByTokenType
		@TokenType
*/

AS

BEGIN

INSERT INTO [dbo].[UserTokens]
           ([Token]
           ,[UserId]
           ,[TokenType])
     VALUES
           (@Token
           ,@UserId
           ,@TokenType)
END


