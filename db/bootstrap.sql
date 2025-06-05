CREATE DATABASE ClienteCrud;

USE ClienteCrud;
CREATE TABLE Clientes(
     Id         INT PRIMARY KEY IDENTITY
    ,DtCriacao  DATETIME DEFAULT GETDATE()
    ,Ativo      BIT DEFAULT 1
    ,Nome       NVARCHAR(100) NOT NULL
    ,Sexo       NVARCHAR(20)
    ,Endereco   NVARCHAR(200)
);

CREATE TABLE Telefones(
     Id         INT PRIMARY KEY IDENTITY
    ,DtCriacao  DATETIME DEFAULT GETDATE()
    ,Ativo      BIT DEFAULT 1
    ,Numero     NVARCHAR(20)
    ,ClienteId  INT
);

ALTER TABLE Telefones 
    ADD CONSTRAINT FK_Telefones_Clientes 
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id);