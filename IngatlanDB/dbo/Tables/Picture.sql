CREATE TABLE [dbo].[Picture] (
    [Id]  INT   IDENTITY (1, 1) NOT NULL,
    [Pic] IMAGE NULL,
    CONSTRAINT [PK_Picture] PRIMARY KEY CLUSTERED ([Id] ASC)
);



