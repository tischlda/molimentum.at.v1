CREATE TABLE [dbo].[Post]
(
	[ID] uniqueidentifier not null default(newsequentialid()),
	[PostCategoryID] uniqueidentifier null,
	[AuthorID] uniqueidentifier not null,
	[Title] nvarchar(100) not null,
	[Body] ntext not null,
	[Latitude] float null,
	[Longitude] float null,
	[PublishDate] datetime,
	[LastUpdatedTime] datetime,
	[IsPublished] bit not null
);
