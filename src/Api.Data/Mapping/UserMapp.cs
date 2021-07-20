using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Mapping
{
    //Mapeamento e configuração da tabela User
    public class UserMapp : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<UserEntity> builder)
        {
           builder.ToTable("User");
           
           builder.HasKey( p => p.ID);

           builder.HasIndex( p => p.Email)
                  .IsUnique();

           builder.Property(u => u.Name)
                  .IsRequired()
                  .HasMaxLength(60);


           builder.Property(u => u.Email)
                  .HasMaxLength(100);       
           
        }
    }
}