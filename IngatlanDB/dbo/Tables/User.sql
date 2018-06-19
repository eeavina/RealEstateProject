CREATE TABLE [dbo].[User] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [UserName]   VARCHAR (50)  NOT NULL,
    [Email]      VARCHAR (50)  NOT NULL,
    [Password]   VARCHAR (100) NOT NULL,
    [UserRoleId] INT           NOT NULL,
    [Salt]       VARCHAR (50)  NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_User_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([Id])
);





