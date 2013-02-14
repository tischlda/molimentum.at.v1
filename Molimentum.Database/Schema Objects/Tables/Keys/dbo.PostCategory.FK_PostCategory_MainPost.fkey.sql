ALTER TABLE [dbo].[PostCategory]
	ADD CONSTRAINT [FK_PostCategory_MainPost] 
	FOREIGN KEY ([MainPostID])
	REFERENCES [Post] ([ID])	

