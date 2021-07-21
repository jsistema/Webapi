using System;
using Api.CrossCutting.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace application
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
            // Configura injeção de dependência da camanda CrossCutting
            ConfigureService.ConfigureDependenciesService(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            services.AddControllers();

            // Configura Swagger da API
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo{
                    Version = "v1",
                    Title = "SaS",
                    Description = "Sistema de Administração Simplificada",
                    TermsOfService = new Uri("http://www.jsistemas.com.br/terms"),
                    Contact = new OpenApiContact {
                        Name = "João Paulo Magalhães",
                        Email = "j.sistemas@hotmail.com",
                        Url = new Uri("http://www.jsistemas.com.br"),
                    },
                    License = new OpenApiLicense 
                    {
                        Name = "Termos de Licença de Uso",
                        Url = new Uri("http://www.jsistemas.com.br/registro")
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Habilitando o Swagger para a API
            app.UseSwagger();
            app.UseSwaggerUI(c => {
               c.SwaggerEndpoint("/swagger/v1/swagger.json","SaS API");
               c.RoutePrefix = string.Empty; 
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
