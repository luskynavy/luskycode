USE [D:\D\PROG\LUSKYCODE\MVCAPPLICATION3 MVC4\MVCAPPLICATION3 MVC4\APP_DATA\DATABASE1.MDF]
GO

/****** Object: Table [dbo].[Borrowed] Script Date: 13/04/2023 13:16:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Borrowed] (
    [id]     INT           IDENTITY (1, 1) NOT NULL,
    [bookId] INT           NOT NULL,
    [date]   DATETIME2 (7) NULL
);


