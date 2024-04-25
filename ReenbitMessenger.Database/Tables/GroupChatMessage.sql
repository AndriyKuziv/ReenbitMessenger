CREATE TABLE [dbo].[GroupChatMessage] (
    [Id] BIGINT PRIMARY KEY IDENTITY NOT NULL,
    [SenderUserId] NVARCHAR (450) NOT NULL,
    [GroupChatId] UNIQUEIDENTIFIER NOT NULL,
    [Text] NVARCHAR (MAX) NOT NULL,
    [SentTime] DATETIME NOT NULL, 
    [MessageToReplyId] BIGINT NULL,
    CONSTRAINT [FK_GroupChatMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_GroupChatMessage_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [dbo].[GroupChat] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_GroupChatMessage_MessageToReply] FOREIGN KEY ([MessageToReplyId]) REFERENCES [dbo].[GroupChatMessage] ([Id])
);

