SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE InitParentChildTables
AS
BEGIN
	SET NOCOUNT ON;

	IF OBJECT_ID('[dbo].[SampleParent]', 'U') IS NULL
	BEGIN
		CREATE TABLE [dbo].[SampleParent]
		(
			ParentID INT PRIMARY KEY IDENTITY,
			ParentName NVARCHAR(64) NOT NULL
		)
	END

	IF OBJECT_ID('[dbo].[SampleChild]', 'U') IS NULL
	BEGIN
		CREATE TABLE [dbo].[SampleChild]
		(
			ChildID INT PRIMARY KEY IDENTITY,
			ParentID INT NOT NULL,
			ChildName NVARCHAR(64) NOT NULL
		)
	END

	DELETE FROM [dbo].[SampleParent]
	DELETE FROM [dbo].[SampleChild]
END
GO
