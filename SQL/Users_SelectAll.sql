USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectAll]    Script Date: 12/31/2022 1:46:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Proc to grab all users info by pages.
-- Code Reviewer: Sorin W.

ALTER proc [dbo].[Users_SelectAll] 
			@PageIndex int
			,@PageSize int

/*
Declare 
@PageIndex int = 0
,@PageSize int = 5

Execute dbo.Users_SelectAll 
		@PageIndex
		,@PageSize
*/

AS

BEGIN

Declare @offset int = @PageIndex * @PageSize

SELECT [Id]
      ,[Email]
      ,[FirstName]
      ,[LastName]
      ,[Mi]
      ,[AvatarUrl]
      ,[IsConfirmed]
      ,[StatusId]
      ,[DateCreated]
      ,[DateModified]
	  ,[TotalCount] = COUNT(1) OVER()
  FROM [dbo].[Users]
  ORDER BY Id

  OFFSET @offset Rows
  Fetch Next @PageSize Rows ONLY

END


