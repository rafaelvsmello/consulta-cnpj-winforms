CREATE DATABASE cadastro;

CREATE TABLE empresa(
    id  INT NOT NULL IDENTITY(1,1),
    cnpj VARCHAR(20),
    situacao VARCHAR(20),
    dataSituacao DATETIME,
    nomeEmpresarial VARCHAR(100),
    nomeFantasia VARCHAR(100),
    endereco VARCHAR(80),
    complemento VARCHAR(80),
    bairro VARCHAR(80),
    cidade VARCHAR(80),
    cep VARCHAR(20),
    dataAbertura DATETIME,
    PRIMARY KEY (id)
);