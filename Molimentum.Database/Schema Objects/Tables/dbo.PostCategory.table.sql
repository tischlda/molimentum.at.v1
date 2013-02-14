CREATE TABLE [dbo].[PostCategory]
(
	[ID] uniqueidentifier not null,
	[MainPostID] uniqueidentifier null,
	[Title] nvarchar(100) not null
)
