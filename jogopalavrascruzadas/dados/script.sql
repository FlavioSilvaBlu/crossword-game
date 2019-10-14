CREATE TABLE [dbo].[palavracruzada]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [palavra] VARCHAR(80) NULL, 
    [dica] VARCHAR(200) NULL, 
    [dat_cad] TIMESTAMP NULL, 
    [dat_alt] TIMESTAMP NULL

)
