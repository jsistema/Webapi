using System;
using System.Collections.Generic;
using Api.CrossCutting.DependencyInjection;
using Api.CrossCutting.Mappings;
using Api.Data.Context;
using Api.Domain.Secuity;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
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

            //Configura os mapeamentos.
            var config = new AutoMapper.MapperConfiguration(cfg => {
               cfg.AddProfile(new DtoToModelProfile());  
               cfg.AddProfile(new EntityToDtoProfile());
               cfg.AddProfile(new ModelToEntityProfile());   
            });

            IMapper mapper = config.CreateMapper();
            services.AddSingleton(mapper);


            //Configura injeção de dependencias para as classes JWT.
            var sigingConfigurations = new SigningConfiguration();
            services.AddSingleton(sigingConfigurations);

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>
                (
                 Configuration.GetSection("TokenConfigurations") //COnfiguração TokensConfigurations de appsettings.json
                ).Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            // adiciona a autenticação ao servico;
            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(beareOptions => 
            {
                var paramsValidation = beareOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = sigingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            //Adiciona a autorização ao servico.    
            services.AddAuthorization(auth => {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build()
                );
            });

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

                //Adiciona botão authorize no Swagger;
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                //Informar ao swagger o comando de utilização do token passado no authorize.
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                  {   
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>()
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

            // if (Environment.GetEnvironmentVariable("MIGRATION").ToLower() == "APLICAR".ToLower())
            // {
            //  using(var service = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
            //  .CreateScope()) 
            //  {
            //      //criando o contexto para a migracao
            //      using (var context = service.ServiceProvider.GetService<MyContext>())
            //      {
            //          //Roda a migration
            //          context.Database.Migrate();
            //      }   
            //  }   
            // }
        }
    }
}
