using Microsoft.AspNetCore.Builder;
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

 *  ===============================================
 */