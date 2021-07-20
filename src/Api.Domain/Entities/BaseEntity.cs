using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Entities
{
    //Classe abastrata para servir como base para outra classe, ou seja, permitir Herança.
    public abstract class BaseEntity
    {
        [Key]
       public Guid ID { get; set; } 

       private DateTime? _createAt;
      
       public DateTime? CreateAt
       {
           get { return _createAt; }
           set { _createAt = (value == null ? DateTime.UtcNow : value); }
       }

       public DateTime? UpdateAt { get; set; }       

    }
}