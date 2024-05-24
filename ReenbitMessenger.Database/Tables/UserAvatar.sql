﻿CREATE TABLE [dbo].[UserAvatar]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(450) NOT NULL UNIQUE, 
    [AvatarUrl] NVARCHAR(450) NOT NULL UNIQUE, 
    CONSTRAINT [FK_GroupChatMessage_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
)
