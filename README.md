# LetShare Auth API

A **LetShare Auth API** é uma API RESTful desenvolvida com **.NET 8** e **PostgreSQL**, com autenticação baseada em **JWT (JSON Web Tokens)**. O objetivo dessa API é permitir a autenticação de usuários e a geração de tokens para garantir a segurança e o acesso controlado aos recursos da aplicação.

---

## Descrição do Projeto

Este projeto foi criado como parte de um desafio técnico para a implementação de autenticação de usuários com JWT. A API oferece endpoints para autenticação de usuários, geração de tokens de acesso e refresh tokens.

### Funcionalidades Principais

- **Autenticação de Usuários**: O usuário envia suas credenciais (usuário e senha) para receber um **access_token** e um **refresh_token**.
- **Renovação de Tokens**: Usando o **refresh_token**, o usuário pode renovar seu **access_token** sem precisar fazer login novamente.

A API utiliza um banco de dados **PostgreSQL** para armazenar as informações dos usuários e verifica as credenciais para gerar os tokens de autenticação. Ela foi construída de forma segura, utilizando práticas recomendadas para a geração de JWT.

---

## Tecnologias Utilizadas

- **.NET 8**: Framework de desenvolvimento para APIs RESTful.
- **PostgreSQL**: Banco de dados relacional utilizado para armazenar os dados dos usuários.
- **JWT (JSON Web Token)**: Tecnologia para geração de tokens de autenticação.
- **Entity Framework Core**: ORM (Object-Relational Mapper) para interagir com o banco de dados PostgreSQL.

---

## Como Rodar o Projeto Localmente

### 1. Clonando o Repositório

Clone o repositório em sua máquina local:

```bash
git clone https://github.com/MatheusTerr/letshare-auth-api.git
cd letshare-auth-api
```

### 2. Instalando as Dependências

Certifique-se de que o SDK do **.NET 8** está instalado em sua máquina. Você pode baixá-lo no site oficial do [.NET](https://dotnet.microsoft.com/).

Restaure as dependências do projeto:

```bash
dotnet restore
```

### 3. Configuração do Banco de Dados

Certifique-se de ter o **PostgreSQL** instalado em sua máquina. Você pode obtê-lo em [PostgreSQL Downloads](https://www.postgresql.org/download/).

No arquivo `appsettings.json`, configure a string de conexão com o seu banco de dados PostgreSQL. Exemplo:

```json
"ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=letshare;Username=postgres;Password=123"
}
```

Substitua o valor de `Password` pelo valor correto da sua configuração local.

### 4. Configuração do JWT

No mesmo arquivo `appsettings.json`, ajuste a chave secreta para geração dos tokens JWT:

```json
"JwtSettings": {
    "SecretKey": "MinhaUltraSecretaKey@123!"
}
```

Recomenda-se alterar a chave para uma mais segura, gerada por você.

### 5. Migrando o Banco de Dados

Execute as migrações do banco de dados para criar as tabelas necessárias:

```bash
dotnet ef database update
```

### 6. Executando a Aplicação

Após configurar o banco de dados e as dependências, execute o projeto:

```bash
dotnet run
```

A API será executada em `http://localhost:5000` por padrão.

---

## Endpoints da API

### 1. Autenticação (Geração de Tokens)

**POST** `/api/auth/token`

#### Request

```json
{
    "username": "assessment@letshare-test.com",
    "password": "senhaDoUsuario",
    "grant_type": "password",
    "client_id": "web",
    "client_secret": "webpass1"
}
```

#### Response (Sucesso)

```json
{
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Response (Erro)

```json
{
    "message": "Usuário ou senha inválidos."
}
```

### 2. Renovação de Tokens

**POST** `/api/auth/refresh`

#### Request

```json
{
    "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Response (Sucesso)

```json
{
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### Response (Erro)

```json
{
    "message": "Refresh token inválido ou expirado."
}
```

## Dependências

- **.NET 8**: Framework utilizado para construir a API.
- **Entity Framework Core**: ORM utilizado para interação com o PostgreSQL.
- **Npgsql**: Provedor de dados para PostgreSQL em .NET.
- **Microsoft.AspNetCore.Authentication.JwtBearer**: Pacote para suporte à autenticação via JWT.
- **Swashbuckle.AspNetCore**: Para a geração automática de documentação Swagger da API.

---
