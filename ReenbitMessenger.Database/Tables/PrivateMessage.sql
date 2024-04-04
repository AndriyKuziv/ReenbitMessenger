CREATE TABLE [dbo].[PrivateMessage]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [SenderUserId] NVARCHAR (450) NOT NULL, 
    [ReceiverUserId] NVARCHAR (450) NOT NULL,  
    [Text] NVARCHAR(MAX) NOT NULL,
    [MessageToReplyId] BIGINT NULL, 
    CONSTRAINT [FK_PrivateMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_PrivateMessage_ReceiverUser] FOREIGN KEY ([ReceiverUserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_PrivateMessage_MessageToReply] FOREIGN KEY ([MessageToReplyId]) REFERENCES [PrivateMessage]([Id])
)
