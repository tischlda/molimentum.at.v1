ALTER TABLE [dbo].[PostTag]
	ADD CONSTRAINT [FK_PostTag_Post] 
	FOREIGN KEY ([PostID])
	REFERENCES [Post] ([ID])	

