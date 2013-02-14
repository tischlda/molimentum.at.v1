CREATE TABLE [dbo].[PostComment]
(
	[ID] uniqueidentifier not null,
	[PostID] uniqueidentifier not null,
	[Author] nvarchar(100) not null,
	[Title] nvarchar(100) not null,
	[Body] ntext not null,
	[Website] nvarchar(100) null,
    [EMail] nvarchar(100) null,
	[PublishDate] datetime
)
