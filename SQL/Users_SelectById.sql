USE [Migrately]
GO
/****** Object:  StoredProcedure [dbo].[Users_SelectById]    Script Date: 12/31/2022 1:46:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Author: Jonathan S.
-- Create date: 11/16/2022
-- Description: Proc to grab user info by Id.
-- Code Reviewer: Sorin W.

ALTER proc [dbo].[Users_SelectById]
			@Id int

/*
Declare @Id int = 1

Execute dbo.Users_SelectById
		@Id
*/

AS

BEGIN

		SELECT u.[Id] as UserId
			,[FirstName]
			,[Mi]
			,[LastName]			
			,[AvatarUrl]
			,[Email]
			,st.Id
			,st.[Name] as Status
			,Role = (
						Select	r.Id
								,r.Name
						From dbo.Roles as r inner join dbo.UserRoles as ur
								on r.Id = ur.RoleId
						Where u.Id = ur.UserId
						For JSON AUTO
			)
			FROM [dbo].[Users] as u inner join dbo.StatusTypes as st
							on u.StatusId = st.Id
			WHERE u.Id = @Id

END


