CREATE DATABASE ClienteCrud;

USE ClienteCrud;
CREATE TABLE Cliente(
     Id         INT PRIMARY KEY IDENTITY
    ,DtCriacao  DATETIME DEFAULT GETDATE()
    ,Ativo      BIT DEFAULT 1
    ,Nome       NVARCHAR(100) NOT NULL
    ,Sexo       NVARCHAR(20)
    ,Endereco   NVARCHAR(200)
);

CREATE TABLE Telefone(
     Id         INT PRIMARY KEY IDENTITY
    ,DtCriacao  DATETIME DEFAULT GETDATE()
    ,Ativo      BIT DEFAULT 1
    ,Numero     NVARCHAR(20)
    ,ClienteId  INT
);

ALTER TABLE Telefone 
    ADD CONSTRAINT FK_Telefone_Cliente 
    FOREIGN KEY (ClienteId) REFERENCES Cliente(Id);