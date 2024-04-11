CREATE TABLE [dbo].[GroupChatMembers]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [GroupChatId] CHAR(36) NOT NULL, 
    [UserId] NVARCHAR (450) NOT NULL, 
    [GroupChatRoleId] TINYINT NOT NULL, 
    CONSTRAINT [FK_GroupChatMembers_GroupChat] FOREIGN KEY ([GroupChatId]) REFERENCES [GroupChat]([Id]), 
    CONSTRAINT [FK_GroupChatMembers_User] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_GroupChatMembers_Role] FOREIGN KEY ([GroupChatRoleId]) REFERENCES [GroupChatRoles]([Id]) 
)
