﻿CREATE TABLE [dbo].[GroupChatMessage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SenderUserId] NVARCHAR(450) NOT NULL, 
    [GroupChatId] CHAR(36) NOT NULL,  
    [Text] NVARCHAR(MAX) NOT NULL,
    [MessageToReplyId] BIGINT NULL, 
    CONSTRAINT [FK_GroupChatMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_GroupChatMessage_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat]([Id]), 
    CONSTRAINT [FK_GroupChatMessage_MessageToReply] FOREIGN KEY ([MessageToReplyId]) REFERENCES [GroupChatMessage]([Id])
)