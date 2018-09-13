CREATE TABLE [dbo].[Product] (
    [ProductId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [Code]      NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);



