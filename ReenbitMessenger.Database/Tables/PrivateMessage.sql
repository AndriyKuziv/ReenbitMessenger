CREATE TABLE [dbo].[PrivateMessage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SenderUserId] CHAR(36) NOT NULL, 
    [ReceiverUserId] CHAR(36) NOT NULL,  
    [Text] NVARCHAR(MAX) NOT NULL,
    [MessageToReplyId] BIGINT NULL, 
    CONSTRAINT [FK_PrivateMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_PrivateMessage_ReceiverUser] FOREIGN KEY ([ReceiverUserId]) REFERENCES [User]([Id]), 
    CONSTRAINT [FK_PrivateMessage_MessageToReply] FOREIGN KEY ([MessageToReplyId]) REFERENCES [PrivateMessage]([Id])
)
