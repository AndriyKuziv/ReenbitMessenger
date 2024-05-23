CREATE TABLE [dbo].[UserAvatar]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(450) NULL, 
    [AvatarUrl] NVARCHAR(450) NULL, 
    CONSTRAINT [FK_GroupChatMessage_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
