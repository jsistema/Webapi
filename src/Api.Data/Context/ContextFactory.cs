using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Api.Data.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<MyContext>
    {
        public MyContext CreateDbContext(string[] args)
        {
            //Usado para criar migrações em tempo de projeto 
           var connectionString = "Server=localhost;Port=3306;Database=dbAPI;Uid=root;Pwd=Jp.08429675";
           var optionsBuilder = new DbContextOptionsBuilder<MyContext> ();
           optionsBuilder.UseMySql (connectionString);
           return new MyContext (optionsBuilder.Options);
        }
    }
}