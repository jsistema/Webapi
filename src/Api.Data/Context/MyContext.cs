using System;
using Api.Data.Mapping;
using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Context
{
    public class MyContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public MyContext (DbContextOptions<MyContext> options) : base (options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity> (new UserMapp().Configure);

            //Implementa um seeding para migrations inicial, com a inclusão do usuário padrão.    
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity {
                    ID = Guid.NewGuid(),
                    Name = "Administrador",
                    Email = "admin@admin.com",
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                }
            );

        }
    }
}