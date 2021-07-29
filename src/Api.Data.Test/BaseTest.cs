using System;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Api.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Test
{
    public abstract class BaseTest
    {

        public BaseTest()
        {
            
        }

    }


    public class DbTest : IDisposable
    {

        // Para qualquer teste, será  criado um banco de dados
        private string dataBaseName = $"dbApiTest_{Guid.NewGuid().ToString().Replace("-",string.Empty)}";

        public ServiceProvider ServiceProvider {get; private set;}


        public DbTest()
        {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<MyContext>(o=> o.UseMySql($"Persist Security Info=True;Server=localhost;Database={dataBaseName};User=root;Password=max@5699"),
            ServiceLifetime.Transient);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            using (var context = ServiceProvider.GetService<MyContext>())
            {
                context.Database.EnsureCreated();
            }

        }

        // Metodo dispose, para quando o objeto for liberado da memória
        public void Dispose() {
            
            using (var context = ServiceProvider.GetService<MyContext>())
            {
                context.Database.EnsureDeleted();
            }

        }

    }

}
