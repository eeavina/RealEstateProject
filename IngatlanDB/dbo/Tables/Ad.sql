CREATE TABLE [dbo].[Ad] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (100) NOT NULL,
    [District]    VARCHAR (5)   NOT NULL,
    [Street]      VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (1000) NOT NULL,
    [Size]        INT           NOT NULL,
    [Price]       INT           NOT NULL,
    [PictureId]   INT           NULL,
    [UserId]      INT           NOT NULL,
    CONSTRAINT [PK_Ad] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Ad_Picture] FOREIGN KEY ([PictureId]) REFERENCES [dbo].[Picture] ([Id]),
    CONSTRAINT [FK_Ad_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);



