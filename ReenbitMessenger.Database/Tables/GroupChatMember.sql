CREATE TABLE [dbo].[GroupChatMember]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupChatId] CHAR(36) NOT NULL, 
    [UserId] NVARCHAR (450) NOT NULL, 
    [GroupChatRoleId] TINYINT NOT NULL, 
    CONSTRAINT [FK_GroupChatMember_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat]([Id]), 
    CONSTRAINT [FK_GroupChatMember_User] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_GroupChatMember_Role] FOREIGN KEY ([GroupChatRoleId]) REFERENCES [GroupChatRole]([Id]) 
)
