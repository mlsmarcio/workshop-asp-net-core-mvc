﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;
using SalesWebMvc.Data;
using SalesWebMvc.Services;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                builder.MigrationsAssembly("SalesWebMvc")));
            //        options.UseSqlServer(Configuration.GetConnectionString("SalesWebMvcContext")));

            // REGISTRANDO O SERVIÇO NO SISTEMA DE INJEÇÃO DE DEPENDÊNCIA DA APLICAÇÃO - POPULA A BASE DE DADOS
            services.AddScoped<SeedingService>();

            // REGISTRANDO O SERVIÇO SellerService
            services.AddScoped<SellerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seedingService.Seed();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

/*
 *  ===============================================
 *  1 - Criar o Scalffolded
 *      - Na pasta controller, botão direito, new scalffolded item
 *      - Selecionar MVC Controller with views, using Entity Framework
 *      - Selecionar a classe de Model  ex.: Department
 *      - Criar a classe Data context
 *        - a ferramenta criará as classes necessárias para o crud
 *          - SalesWebMvcContext - na pasta Data
 *          - DepartmentsController - na pasta Controller
 *        - Cria também as views: Create, Delete, Details, Edit e Index 
 *        - Cria a string de conexão em  appsettings.json 
 *      
 *  3 - Instalação do PROVIDER MYSQL 
 *      - No Package Manager Console - 
 *        Execute o comando especificando a versão 2.1.1:
            Install-Package
            Pomelo.EntityFrameworkCore.MySql -Version 2.1.1

          Ou então instale usando a interface do Visual Studio:
            Botão direito no nome do projeto -> Manage NuGet Packages
            Na aba Browse, pesquise por: Pomelo.EntityFrameworkCore.MySql
            Clique uma vez no resultado Pomelo.EntityFrameworkCore.Mysql e, na janela ao lado, escolha a versão 2.1.1
            Clique em Install e conclua a instalação.
 *  
 *  2 - ADAPTANDO O PROJETO PARA O BANCO DE DADOS MYSQL
 *      - Corrigir a string de conexão para o banco de dados no arquivo appsettings.json
 *          
 *          "SalesWebMvcContext": "server=localhost;userid=developer;password=oicram;database=saleswebmvcappdb"
 *          
 *      - Corrigir na classe Startup.cs a definição de DbContext para injeção de dependência
 *        - No método ConfigureServices
 *          
 *          services.AddDbContext<SalesWebMvcContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("SalesWebMvcContext"), builder =>
                builder.MigrationsAssembly("SalesWebMvc")));

        - Compilar para verificar se está funcionando
            CTRL + Shift + B

    3 - Criar a primeira Migration
        - No Package Manager Console
            Add-Migration <nome>

                Serão criadas as classes da Migration:
                  - A pasta Migrations
                    - O arquivo nomeado com a data e nome da migration - Cria a tabela correspondente a classe Model
                    - O arquivo com nome da classe de context e da migration - Com o estado do banco de dados
            
        - Digitar o proximo comando para executar a Migration criada
            Update-Database
            
            Será executada e gerará o banco de dados

    4 - Alterar o tema do bootstrap
        - Acessar o site https://bootswatch.com/3
        - Em temas, selecionar o tema desejado
        - No menu do tema, selecionar o arquivo bootstrap.css
        - Salvar como no diretório wwwroot\lib\bootstrap\dist\css\bootstrap-NOME_TEMA.css
        - Modificar o arquivo Views\Shared\_Layout.cshtml para referência ao novo arquivo css

   5 - Adicionando novas entidades e segunda migration
        - Adicionar a classe em Models
            - Adicionar um Enum
                - Na pasta Models, adicionar uma pasta Enums
            - Adicionar os atributos básicos
            - Adicionar Associações
                Um departamento têm n vendedores, então, na classe Departamento, criar um ICollection de Vendedores:
                    public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

                Um vendedor possui um departamento, então, adicionar o atributo departamento na classe Vendedor:
                    public Department Department { get; set; }
                
                Um vendedor possui n Registros de vendas, adicionar uma coleção de registros na classe Vendedor
                    public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();
                
                Um RegistroDeVendas possui um vendedor, implementar a associação com a propriedade na classe RegistroDeVendas
                    public Seller Seller { get; set; }
                
            - Adicionar Contrutores, padrão e com argumentos
                - O Framework precisa do construtor vasio, default.
                - Adicionar o construtor com parâmetros, mas, não incluir os atributos que são coleções.
        
            - Adicionar os métodos, operações das classes
                - Utilizar linq para obter o total de vendas de um vendedor em um intervalo de datas
                    return Sales.Where(sr => sr.Date >= initial && sr.Date <= final)    //Seleciona os registros do período
                                .Sum(sr => sr.Amount); 

        - Adiconar DbSet's in DbContext
            Na classe Data.SalesWebMvcContext.cs adicionar as propriedades correspondentes as entidades
                public DbSet<Seller> Seller { get; set; }
                public DbSet<SalesRecord> SalesRecord { get; set; }

        - Adicionar uma Migration para as novas entidades
            Na aba, Package Manager Console
                Add-Migration OtherEntities
                - O sistema criará o arquivo da Migration na pasta correspondente

        - Atualizar o banco de dados com o comando:
            Update-Database
            (verificar se o serviço mysql está em execução)

    6 - Criando um serviço para popular a base de dados, usando injeção de dependência
        - No mysql Work Brench, exclua o banco de dados
        - Execute as migrations
                No Package Manager Console
                - Update-Database
        - Stop o IIS
        - Criar a classe SeedingService na pasta Data
        - Registrar essa classe no sistema de injeção de dependência da aplicação, em Startup.cs
            Permite que esse serviço seja injetado na aplicação e que outros serviços sejam injetados nele.
            No método ConfigureServices adicionar:
                services.AddScoped<SeedingService>();
        
        - Implementar a classe SeedingService
            - Criar uma referência ao Context da aplicação
            - Receber um Context no construtor
            - Criar o método que realiza a operação de popular a base de dados
                - Textar se existe algum registro na base de dados, se não existir, popular as tabelas
                - Criar os objetos com dados para popular o BD
                - Utilizar o objeto _context para adicionar os registros
            
        - Fazer a chamada desse método em Startup.cs
            - No método Configure, adicionar o parâmetro do tipo SeedingService
                public void Configure(IApplicationBuilder app, IHostingEnvironment env, SeedingService seedingService)

            - Quando a execução estiver em modo de desenvolvimento chame o método
                seedingService.Seed();

    7 - Criar o Controlador de Vendedores (Sellers), Link na página Home e Views.
        
        - Criar os Links para departamentos e vendedores
            Em, views, shared, _Layout.cshtml
                - O atributo asp-controller deve receber o nome que consta no Controller correspondente: 
                    ex.: Departments para o controller DepartmentsController
                    <li><a asp-area="" asp-controller="Departments" asp-action="Index">Departments</a></li>

        - Criando o Controller de Sellers 'Vendedores'
            - Botão direito na pasta Controllers, Add... Controller...
            - Colocar o nome correspondente ao Link Criado, no atributo asp-controller ex.: Sellers então SellersController
            - Criar a página de Index, na pasta views
                - Criar a pasta Sellers na pasta Views (deve ser igual ao nome no controlador) 
                
            
    8 - Criando a classe de serviço de vendedores - SellersService
    
        - Criar a pasta de serviços no projeto, botão direito no projeto, new folder, nomear como Services
        - Criar a classe de serviço SellerService na pasta de serviços
        - Registrar em Startup.cs no método ConfigureServices o serviço na injeção de dependência do sistema
            services.AddScoped<SellerService>();
        - Implementar um método que retorne todos os vendedores no serviço criado
            
            Criar o atributo e o construtor:
                private readonly SalesWebMvcContext _context;
    
                public SellerService(SalesWebMvcContext context)
                {
                    _context = context;
                }

            Criar o método
                public List<Seller> FindAll()
                {
                    return _context.Seller.ToList();
                }

        - No controller implementar o método Index que deve chamar o método FindAll 
            - Criar uma dependência para o SellerService, um atributo deste tipo
            
            - Utilizar este objeto no método Index para criar uma lista de Sellers (vendedores)
            - Passar a lista criada como parâmetro para o método View que vai gerar um IActionResult 

        - Na página Index escrever um código de template para exibir a lista de vendedores
            - No arquivo Views/Sellers/Index.cshtml, Criar um objeto @model para obter os dados (lista) e mostrar na tela
                @model IEnumerable<SalesWebMvc.Models.Seller>

            - Criar uma tabela e bootstrap para exibir os dados
                @Html.DisplayNameFor(model => model.Name)  - obtém o nome do campo, ou simplesmente digite 'Name'
                
                @foreach (var item in Model)
                        { 
                            <tr>
                                <td>
                                    @Html.DisplayFor(modelItem => item.Name)
                                </td>

 *  ===============================================
 */