using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using VerityNet.Dao.MovimentoManualContext.Repositories;
using VerityNet.Domain.MovimentoManualContext.Commands.Handlers;
using VerityNet.Domain.MovimentoManualContext.Repositories;
using VerityNet.Infra.Data.DataContext;
using VerityNet.Infra.Data.Filters;

namespace VerityNet.UI.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(config =>
            {
                // Configurando o serviço de registro de erros globais
                config.Filters.Add(new ServiceFilterAttribute(typeof(GlobalExceptionFilter)));
            }).AddJsonOptions(options =>
            {
                // Configurando o serviço de retorno dos dados para não retornar valores nulos no Json
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            // Configurando o serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "VerityNet API - Movimentos Manuais",
                    Version = "v1",
                    Description = "Exercício verity para desenvolvedores",
                    Contact = new Contact
                    {
                        Name = "Jamil Abrantes",
                        Url = "https://github.com/JamilEAbrantes"
                    }
                });

                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });
            // Middleware que faz a permissão de compartilhamento de dados entre domínios
            services.AddCors();
            // Middleware que faz a compressão da resposta de dados para o client. (GZIP)
            services.AddResponseCompression();

            /////////////////////////////////////////////////////////////////////////////////////////
            // Registro de Dependências /////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////////////////////////////
            // Modos de vida para um serviço que está sendo injetado:
            // Singleton:   Um objeto do serviço é criado e fornecido para todas as requisições. Assim, todas as requisições obtém o mesmo objeto;
            //    Escope:   Um objeto do serviço é criado para cada requisição. Dessa forma, cada requisição obtém uma nova instância do serviço;
            // Transient:   Um objeto do serviço é criado toda vez que um objeto for requisitado;
            /////////////////////////////////////////////////////////////////////////////////////////
            services.AddTransient<IMovimentoManualRepository, MovimentoManualRepository>();
            services.AddTransient<MovimentoManualHandler, MovimentoManualHandler>();
            services.AddScoped<VerityNetDataContext, VerityNetDataContext>();
            services.AddScoped<GlobalExceptionFilter>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x =>
            {
                x.AllowAnyHeader();
                x.AllowAnyMethod();
                x.AllowAnyOrigin();
            });

            app.UseMvc();
            app.UseResponseCompression();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VerityNet API");
            });
        }
    }
}
