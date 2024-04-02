CREATE TABLE [dbo].[User]
(
	[Id] CHAR(36) NOT NULL PRIMARY KEY, 
    [Username] NVARCHAR(15) NOT NULL, 
    [Email] NVARCHAR(20) NOT NULL, 
    [Password] VARCHAR(20) NOT NULL
)
