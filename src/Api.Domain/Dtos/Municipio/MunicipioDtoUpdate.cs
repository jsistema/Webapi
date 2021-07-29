using System;
using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.Municipio
{
    public class MunicipioDtoUpdate
    {
        
        [Required(ErrorMessage ="O Id do município é campo obrigatório.")]
        public Guid Id { get; set; }
        
        [Required(ErrorMessage ="Nome do município é campo obrigatório.")]
        [StringLength(60, ErrorMessage ="Nome munícpio deve ter no máximo {1}")]
        public string Nome { get; set; }

        [Range(0,int.MaxValue, ErrorMessage ="Código do IBGE é inválido.")]
        public int CodIBGE { get; set; }

        [Required(ErrorMessage ="Estado é obrigatório.")]
        public Guid UfId { get; set; }  
    }
}