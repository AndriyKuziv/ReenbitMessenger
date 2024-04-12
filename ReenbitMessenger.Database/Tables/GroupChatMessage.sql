﻿CREATE TABLE [dbo].[GroupChatMessage] (
    [Id]               BIGINT         PRIMARY KEY NOT NULL,
    [SenderUserId]     NVARCHAR (450) NOT NULL,
    [GroupChatId]      CHAR (36)      NOT NULL,
    [Text]             NVARCHAR (MAX) NOT NULL,
    [SentTime] SMALLDATETIME NOT NULL, 
    [MessageToReplyId] BIGINT         NULL,
    CONSTRAINT [FK_GroupChatMessage_SenderUser] FOREIGN KEY ([SenderUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_GroupChatMessage_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [dbo].[GroupChat] ([Id]),
    CONSTRAINT [FK_GroupChatMessage_MessageToReply] FOREIGN KEY ([MessageToReplyId]) REFERENCES [dbo].[GroupChatMessage] ([Id])
);

