CREATE TABLE [dbo].[Policy] (
    [PolicyId]  BIGINT   IDENTITY (1, 1) NOT NULL,
    [InsuredId] BIGINT   NOT NULL,
    [DateFrom]  DATETIME NOT NULL,
    [DateTo]    DATETIME NOT NULL,
    CONSTRAINT [PK_Policy] PRIMARY KEY CLUSTERED ([PolicyId] ASC),
    CONSTRAINT [FK_Policy_Insured] FOREIGN KEY ([InsuredId]) REFERENCES [dbo].[Insured] ([InsuredId])
);



