CREATE TABLE [dbo].[PolicyProduct] (
    [PolicyId]  BIGINT NOT NULL,
    [ProductId] BIGINT NOT NULL,
    CONSTRAINT [PK_PolicyProduct] PRIMARY KEY CLUSTERED ([PolicyId] ASC, [ProductId] ASC)
);

