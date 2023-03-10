USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_DeleteById]    Script Date: 12/31/2022 1:52:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Jonathan Searcy
-- Create date: 12/16/22
-- Description: deletes a podcast at an id
-- Code Reviewer:Sorin W.


ALTER proc [dbo].[Podcasts_DeleteById]
			@Id int

/*
Declare @Id int = 7
Execute dbo.Podcasts_DeleteById
		@Id
*/

AS

BEGIN

DELETE FROM [dbo].[Podcasts]
      WHERE Id = @Id
END


