CREATE TABLE [dbo].[Insured] (
    [InsuredId] BIGINT         IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (250) NOT NULL,
    [LastName]  NVARCHAR (250) NOT NULL,
    [Number]    NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Insured] PRIMARY KEY CLUSTERED ([InsuredId] ASC)
);



