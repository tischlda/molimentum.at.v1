ALTER TABLE [dbo].[Post]
	ADD CONSTRAINT [FK_Post_PostCategory] 
	FOREIGN KEY ([PostCategoryID])
	REFERENCES [PostCategory] ([ID])	

