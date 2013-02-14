CREATE TABLE [dbo].[PositionReport]
(
	[ID] uniqueidentifier not null default(newsequentialid()),
	[PositionDateTime] datetime not null,
	[Comment] nvarchar(200) not null,
	[Latitude] float not null,
	[Longitude] float not null,
	[Course] float not null,
	[Speed] float not null,
	[WindDirection] float not null,
	[WindSpeed] float not null
);