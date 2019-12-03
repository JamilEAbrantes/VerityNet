GO
-- Criar DB
CREATE DATABASE MovimentosManuais;

GO
-- Acessar DB
USE MovimentosManuais;

GO
-- Criar tabela Produto
CREATE TABLE Produto (
    Cod_Produto CHAR(4) NOT NULL PRIMARY KEY,
    Des_Produto VARCHAR(30),
	Sta_Status CHAR(1)
);

GO
-- Criar tabela ProdutoCosif
CREATE TABLE ProdutoCosif (
	Cod_Produto CHAR(4) NOT NULL FOREIGN KEY REFERENCES Produto(Cod_Produto),
	Cod_Cosif CHAR(11) NOT NULL PRIMARY KEY,
	Cod_Classificacao CHAR(6),
	Sta_Status CHAR(1)	
);

GO
-- Criar tabela MovimentoManual
CREATE TABLE MovimentoManual(
	Dat_Mes NUMERIC(2,0) NOT NULL,
    Dat_Ano NUMERIC(4,0) NOT NULL,
    Num_Lancemanto NUMERIC(18,0) NOT NULL,
    Cod_Produto CHAR(4) NOT NULL FOREIGN KEY REFERENCES Produto(Cod_Produto),
    Cod_Cosif CHAR(11) NOT NULL FOREIGN KEY REFERENCES ProdutoCosif(Cod_Cosif),
    Val_Valor NUMERIC(18,2) NOT NULL,
    Des_Descricao VARCHAR(50) NOT NULL,
    Dat_Movimento SMALLDATETIME NOT NULL,
    Cod_Usuario VARCHAR(15) NOT NULL,
	CONSTRAINT PK_Person PRIMARY KEY (Dat_Mes, Dat_Ano, Num_Lancemanto)
);

GO
-- Produtos
INSERT INTO Produto(Cod_Produto, Des_Produto, Sta_Status) VALUES ('1', 'Produto 1', 'A');
INSERT INTO Produto(Cod_Produto, Des_Produto, Sta_Status) VALUES ('2', 'Produto 2', 'I');
INSERT INTO Produto(Cod_Produto, Des_Produto, Sta_Status) VALUES ('3', 'Produto 3', 'A');

GO
-- Cosifs
INSERT INTO ProdutoCosif(Cod_Produto, Cod_Cosif, Cod_Classificacao, Sta_Status) VALUES ('1', '11', '111', 'A');
INSERT INTO ProdutoCosif(Cod_Produto, Cod_Cosif, Cod_Classificacao, Sta_Status) VALUES ('2', '22', '222', 'A');
INSERT INTO ProdutoCosif(Cod_Produto, Cod_Cosif, Cod_Classificacao, Sta_Status) VALUES ('3', '33', '333', 'A');

GO

-- PROC spListarMovimentosManuais
CREATE PROCEDURE [dbo].[spListarMovimentosManuais]
AS    
BEGIN
	BEGIN TRY    
		BEGIN				
			SELECT			mm.Dat_Mes, mm.Dat_Ano, p.Cod_Produto, p.Des_Produto, mm.Num_Lancemanto, mm.Des_Descricao, mm.Val_Valor
            FROM			MovimentoManual mm
            INNER JOIN		ProdutoCosif pc ON mm.Cod_Cosif = pc.Cod_Cosif
            INNER JOIN		Produto p ON pc.Cod_Produto = p.Cod_Produto
            ORDER BY		mm.Dat_Mes, mm.Dat_Ano, mm.Num_Lancemanto DESC
		END        
	END TRY    
	BEGIN CATCH   
		SELECT ERROR_MESSAGE() AS ErrorMessage;      
	END CATCH        
END
GO

-- PROC spListarProdutos
CREATE PROCEDURE [dbo].[spListarProdutos]
AS    
BEGIN	
	BEGIN TRY    
		BEGIN				
			SELECT Cod_Produto, Des_Produto, Sta_Status FROM Produto ORDER BY Cod_Produto DESC
		END        
	END TRY    
	BEGIN CATCH   
		SELECT ERROR_MESSAGE() AS ErrorMessage;      
	END CATCH        
END
GO

-- PROC spListarProdutosCosif
CREATE PROCEDURE [dbo].[spListarProdutosCosif]  
(  
	@Cod_Cosif CHAR(11),  
	@Sta_Status CHAR(1)  
)  
AS      
BEGIN   
	BEGIN TRY      
		BEGIN      
			SELECT		pc.Cod_Cosif, pc.Cod_Produto, pc.Cod_Classificacao, pc.Sta_Status AS Sta_Cosif,  
						p.Des_Produto, p.Sta_Status AS Sta_Produto  
			FROM		ProdutoCosif pc  
			INNER JOIN	Produto p ON pc.Cod_Produto = p.Cod_Produto  
			WHERE		pc.Cod_Cosif = @Cod_Cosif AND pc.Sta_Status = @Sta_Status  
		END          
	END TRY      
	BEGIN CATCH     
		SELECT ERROR_MESSAGE() AS ErrorMessage;        
	END CATCH          
END
GO

-- PROC spListarProdutosCosifs
CREATE PROCEDURE [dbo].[spListarProdutosCosifs]
AS    
BEGIN	
	BEGIN TRY    
		BEGIN				
			SELECT			pc.Cod_Cosif, pc.Cod_Classificacao, pc.Sta_Status AS Sta_Cosif, 
							p.Cod_Produto, p.Des_Produto, p.Sta_Status AS Sta_Produto
			FROM			ProdutoCosif pc
			INNER JOIN		Produto p ON pc.Cod_Produto = p.Cod_Produto
			ORDER BY		pc.Cod_Cosif DESC
		END        
	END TRY    
	BEGIN CATCH   
		SELECT ERROR_MESSAGE() AS ErrorMessage;      
	END CATCH        
END
GO

-- PROC spObterMovimentoManual
CREATE PROCEDURE [dbo].[spObterMovimentoManual]
(
	@Dat_Mes NUMERIC(2,0),
    @Dat_Ano NUMERIC(4,0),
	@Num_Lancemanto NUMERIC(18,0),
	@Cod_Produto CHAR(4),
	@Cod_Cosif CHAR(11)
)
AS    
BEGIN
	BEGIN TRY    
		BEGIN				
			SELECT			mm.Dat_Mes, mm.Dat_Ano, p.Cod_Produto, p.Des_Produto, mm.Num_Lancemanto, mm.Des_Descricao, mm.Val_Valor
            FROM			MovimentoManual mm
            INNER JOIN		ProdutoCosif pc ON mm.Cod_Cosif = pc.Cod_Cosif
            INNER JOIN		Produto p ON pc.Cod_Produto = p.Cod_Produto
            WHERE			mm.Dat_Mes = @Dat_Mes
                            AND mm.Dat_Ano = @Dat_Ano
                            AND mm.Num_Lancemanto = @Num_Lancemanto
                            AND mm.Cod_Produto = @Cod_Produto
                            AND mm.Cod_Cosif = @Cod_Cosif
		END        
	END TRY    
	BEGIN CATCH   
		SELECT ERROR_MESSAGE() AS ErrorMessage;      
	END CATCH        
END
GO

-- PROC spObterProduto
CREATE PROCEDURE [dbo].[spObterProduto]
	@Cod_Produto CHAR(4),
	@Sta_Status CHAR(1)
AS    
BEGIN
	
	BEGIN TRY    
		BEGIN				
			SELECT Cod_Produto, Des_Produto, Sta_Status FROM Produto WHERE Cod_Produto = @Cod_Produto AND Sta_Status = @Sta_Status
		END        
	END TRY    
	BEGIN CATCH   
		SELECT ERROR_MESSAGE() AS ErrorMessage;      
	END CATCH        
END
GO

-- PROC spObterProximoLancamento
CREATE PROCEDURE [dbo].[spObterProximoLancamento]    
 @Dat_Mes NUMERIC(2,0),    
 @Dat_Ano NUMERIC(4,0),    
 @Cod_Produto CHAR(4),    
 @Cod_Cosif CHAR(11)    
AS        
BEGIN    
 DECLARE @ProximoLancamento INT     
 BEGIN TRY        
  BEGIN        
   SET @ProximoLancamento = ( SELECT TOP 1 Num_Lancemanto    
          FROM MovimentoManual    
          WHERE Dat_Mes = @Dat_Mes    
            AND Dat_Ano = @Dat_Ano    
            AND Cod_Produto = @Cod_Produto    
            AND Cod_Cosif = @Cod_Cosif
			ORDER BY Num_Lancemanto DESC);    
       
     IF @ProximoLancamento IS NULL    
    SELECT ProximoLancamento = 1;    
     ELSE    
    SELECT ProximoLancamento = @ProximoLancamento + 1;    
  END            
 END TRY        
BEGIN CATCH       
 SELECT ERROR_MESSAGE() AS ErrorMessage;          
END CATCH            
END
GO

GO
-- PROC spCriarMovimentoManual
CREATE PROCEDURE [dbo].[spCriarMovimentoManual]      
	@Dat_Mes NUMERIC(2,0),      
    @Dat_Ano NUMERIC(4,0), 
	@Num_Lancemanto NUMERIC(18,0),
	@Cod_Produto CHAR(4),      
	@Cod_Cosif CHAR(11),
	@Val_Valor NUMERIC(18,2), 
	@Des_Descricao VARCHAR(50), 
	@Dat_Movimento SMALLDATETIME, 
	@Cod_Usuario VARCHAR(50)
AS          
BEGIN      
	DECLARE @ProximoLancamento INT       
	BEGIN TRY          
		BEGIN          
			INSERT INTO MovimentoManual (
				Dat_Mes, 
				Dat_Ano, 
				Num_Lancemanto, 
				Cod_Produto, 
				Cod_Cosif, 
				Val_Valor, 
				Des_Descricao, 
				Dat_Movimento, 
				Cod_Usuario)
				VALUES (
				@Dat_Mes, 
				@Dat_Ano, 
				@Num_Lancemanto, 
				@Cod_Produto, 
				@Cod_Cosif, 
				@Val_Valor, 
				@Des_Descricao, 
				@Dat_Movimento, 
				@Cod_Usuario)
		END              
	END TRY          
	BEGIN CATCH         
		SELECT ERROR_MESSAGE() AS ErrorMessage;            
	END CATCH              
END
GO