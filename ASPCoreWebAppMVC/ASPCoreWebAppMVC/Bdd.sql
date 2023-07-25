USE [Library]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Table [dbo].[author]    Script Date: 25/07/2023 11:56:03 ******/

CREATE TABLE [dbo].[author](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](100) NULL,
 CONSTRAINT [PK_author] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[book]    Script Date: 25/07/2023 11:55:11 ******/

CREATE TABLE [dbo].[book](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nchar](100) NULL,
	[note] [nchar](10) NULL,
	[author] [int] NULL,
 CONSTRAINT [PK_book] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[book]  WITH CHECK ADD  CONSTRAINT [FK_Book_ToTable] FOREIGN KEY([author])
REFERENCES [dbo].[author] ([id])
GO

ALTER TABLE [dbo].[book] CHECK CONSTRAINT [FK_Book_ToTable]
GO

/****** Object:  Table [dbo].[borrowed]    Script Date: 25/07/2023 11:57:42 ******/

CREATE TABLE [dbo].[borrowed](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[bookId] [int] NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_borrowed] PRIMARY KEY CLUSTERED
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[borrowed]  WITH CHECK ADD  CONSTRAINT [FK_Borrowed_Book] FOREIGN KEY([bookId])
REFERENCES [dbo].[book] ([id])
GO

ALTER TABLE [dbo].[borrowed] CHECK CONSTRAINT [FK_Borrowed_Book]
GO

