# Projeto ASP.NET Core MVC - Sistema de Vendas

Este é um projeto de introdução ao **ASP.NET Core MVC** utilizando a linguagem **C#** e o banco de dados **MySQL**, acessado por meio do **Entity Framework Core**.

---

## ? Tecnologias Utilizadas

- **.NET Core 2.1**
- **Visual Studio Community 2019**
- **C#**
- **Entity Framework Core**
- **MySQL**

---

## ?? Descrição do Projeto

O sistema simula uma aplicação de cadastro e controle de vendedores vinculados a departamentos, com registro e consulta de vendas.

### ?? Funcionalidades:

- **CRUD completo de Vendedores**
  - Listagem com opções para:
    - Adicionar novo registro
    - Editar
    - Ver detalhes
    - Deletar

- **Consulta de Vendas**
  - Busca **simples**
    - Paginação simples (com paginação tradicional)
  - Busca **agrupada por departamento**
    - Paginação por demanda (scroll dinâmico)
    - ?? *Este recurso foi adicionado além do conteúdo do curso original.*

---

## ??? Estrutura Relacional

- Cada **vendedor** pertence a um **departamento**.
- Cada **venda** está relacionada a um **vendedor**.

---

## ?? Imagens do Projeto

*(Adicione capturas de tela das telas de CRUD e busca, se desejar)*

---

## ?? Como Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/seu-usuario/seu-repositorio.git
