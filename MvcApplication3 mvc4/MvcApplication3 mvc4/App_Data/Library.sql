
CREATE TABLE [dbo].[Author] (
    [Id]   INT         NOT NULL IDENTITY,
    [Name] NCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Book] (
    [id]   INT        IDENTITY (1, 1) NOT NULL,
    [name] NCHAR (20) NULL,
    [note] NCHAR (10) NULL,
    [author] INT NULL, 
    CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED ([id] ASC), 
    CONSTRAINT [FK_Book_ToTable] FOREIGN KEY ([author]) REFERENCES [Author]([id])
);

CREATE TABLE [dbo].[Borrowed] (
    [id]     INT           IDENTITY (1, 1) NOT NULL,
    [bookId] INT           NOT NULL,
    [date]   DATETIME2 (7) NULL,
    CONSTRAINT [PK_Borrowed] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_Borrowed_Book] FOREIGN KEY ([bookId]) REFERENCES [dbo].[Book] ([id])
);


