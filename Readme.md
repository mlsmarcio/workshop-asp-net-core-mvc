# Projeto ASP.NET Core MVC - Sistema de Vendas

Este � um projeto de introdu��o ao **ASP.NET Core MVC** utilizando a linguagem **C#** e o banco de dados **MySQL**, acessado por meio do **Entity Framework Core**.

---

## ? Tecnologias Utilizadas

- **.NET Core 2.1**
- **Visual Studio Community 2019**
- **C#**
- **Entity Framework Core**
- **MySQL**

---

## ?? Descri��o do Projeto

O sistema simula uma aplica��o de cadastro e controle de vendedores vinculados a departamentos, com registro e consulta de vendas.

### ?? Funcionalidades:

- **CRUD completo de Vendedores**
  - Listagem com op��es para:
    - Adicionar novo registro
    - Editar
    - Ver detalhes
    - Deletar

- **Consulta de Vendas**
  - Busca **simples**
    - Pagina��o simples (com pagina��o tradicional)
  - Busca **agrupada por departamento**
    - Pagina��o por demanda (scroll din�mico)
    - ?? *Este recurso foi adicionado al�m do conte�do do curso original.*

---

## ??? Estrutura Relacional

- Cada **vendedor** pertence a um **departamento**.
- Cada **venda** est� relacionada a um **vendedor**.

---

## ?? Imagens do Projeto

*(Adicione capturas de tela das telas de CRUD e busca, se desejar)*

---

## ?? Como Executar

1. Clone o reposit�rio:
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
