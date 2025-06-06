# Projeto ASP.NET Core MVC - Sistema de Vendas

Este � um projeto de introdu��o ao **ASP.NET Core MVC** utilizando a linguagem **C#** e o banco de dados **MySQL**, acessado por meio do **Entity Framework Core**.

---

## Tecnologias Utilizadas

- **.NET Core 2.1**
- **Visual Studio Community 2019**
- **C#**
- **Entity Framework Core**
- **MySQL**

---

## Descri��o do Projeto

O sistema simula uma aplica��o de cadastro e controle de vendedores vinculados a departamentos, com registro e consulta de vendas.

### Funcionalidades:

- **CRUD completo de Vendedores**
  - Listagem com op��es para:
    - Adicionar novo registro
    - Editar
    - Ver detalhes
    - Deletar

- **Consulta de Vendas**
  - Busca **simples**
    - Pagina��o simples (pagina��o tradicional)
    
  - Busca **agrupada por departamento**
    - Pagina��o por demanda (scroll din�mico)
    
  - *Estes recursos de pagina��o foram adicionados ao conte�do dado no curso.*

---

## Estrutura Relacional

- Cada **vendedor** pertence a um **departamento**.
- Cada **venda** est� relacionada a um **vendedor**.

---

## Imagens do Projeto

### Tela de Listagem de Vendedores
![Tela de Listagem](SalesWebMvc/wwwroot/images/Vendedores.PNG)

### Tela de Busca de Vendas
![Tela de Busca](SalesWebMvc/wwwroot/images/BuscaDeVendas.PNG)

### Tela de Listagem de Vendas - Com pagina��o
![Tela de Busca](SalesWebMvc/wwwroot/images/BuscaSimplesComPagina��o.PNG)

### Tela de Listagem Agrupada por Departamento - Com pagina��o por demanda
![Tela de Busca](SalesWebMvc/wwwroot/images/BuscaAgrupadaComPaginacaoScroll.PNG)

---

## Como Executar

1. Clone o reposit�rio:
   ```bash
   git clone https://github.com/mlsmarcio/workshop-asp-net-core-mvc.git

Abra o projeto no Visual Studio 2019.

Configure a string de conex�o com o MySQL no appsettings.json.

Aplique as migrations do Entity Framework:
```bash
Update-Database
