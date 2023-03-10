USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Podcasts_Insert]    Script Date: 12/31/2022 1:52:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Author: Jonathan S.
-- Create date: 12/15/22
-- Description: Inserts a new podcast
-- Code Reviewer:Sorin W.


ALTER proc [dbo].[Podcasts_Insert]
			@Title nvarchar(100)
           ,@Description nvarchar(100)
           ,@Link nvarchar(255)
           ,@CoverPhoto nvarchar(255)
           ,@CreatedBy int
		   ,@Id int OUTPUT

/*
	Declare 
			@Title nvarchar(100) = 'New Pod'
           ,@Description nvarchar(100) = 'A quick little cast'
           ,@Link nvarchar(255) = 'newlink.com'
           ,@CoverPhoto nvarchar(255) = 'superimg.com'
           ,@CreatedBy int = 32
		   ,@Id int

	 Execute dbo.Podcasts_Insert  
			@Title 
           ,@Description 
           ,@Link 
           ,@CoverPhoto 
           ,@CreatedBy
		   ,@Id OUTPUT
*/

AS

BEGIN

INSERT INTO [dbo].[Podcasts]
           ([Title]
           ,[Description]
           ,[Link]
           ,[CoverPhoto]
           ,[CreatedBy]
           )
     VALUES
           (@Title 
           ,@Description 
           ,@Link 
           ,@CoverPhoto 
           ,@CreatedBy 
           ) 

	SET @Id = SCOPE_IDENTITY()

END

