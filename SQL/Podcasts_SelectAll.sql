USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_SelectAll]    Script Date: 12/31/2022 1:52:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Jonathan Searcy
-- Create date: 12/16/22
-- Description: Selects all podcast
-- Code Reviewer:Sorin W.


ALTER proc [dbo].[Podcasts_SelectAll]

/*
Execute dbo.Podcasts_SelectAll
*/

AS

BEGIN

SELECT [Id]
      ,[Title]
      ,[Description]
      ,[Link]
      ,[CoverPhoto]
      ,[CreatedBy]
  FROM [dbo].[Podcasts]

END


