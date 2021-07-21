using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage ="O e-mail é um campo obrigatório para o login.")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        [StringLength(100, ErrorMessage ="O tamanho máximo do e-mail é inválido. Máximo {1} caracteres.")]
        public string Email { get; set; }

    }
}