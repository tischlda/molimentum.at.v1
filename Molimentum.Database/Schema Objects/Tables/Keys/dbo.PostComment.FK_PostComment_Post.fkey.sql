ALTER TABLE [dbo].[PostComment]
	ADD CONSTRAINT [FK_PostComment_Post] 
	FOREIGN KEY ([PostID])
	REFERENCES [Post] ([ID])	

