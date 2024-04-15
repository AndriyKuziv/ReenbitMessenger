CREATE TABLE [dbo].[GroupChatMember]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupChatId] UNIQUEIDENTIFIER NOT NULL, 
    [UserId] NVARCHAR (450) NOT NULL, 
    [GroupChatRoleId] TINYINT NOT NULL, 
    CONSTRAINT [FK_GroupChatMember_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat]([Id]) ON DELETE CASCADE, 
    CONSTRAINT [FK_GroupChatMember_User] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_GroupChatMember_Role] FOREIGN KEY ([GroupChatRoleId]) REFERENCES [GroupChatRole]([Id]) 
)
