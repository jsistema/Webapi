using System.ComponentModel.DataAnnotations;

namespace Api.Domain.Dtos.User
{
    public class UserDtoCreate
    {
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        [StringLength(60, ErrorMessage ="O nome deve ter no máximo {1} caracteres")]
        public string Name { get; set; }

        [Required(ErrorMessage ="O e-mail é um campo obrigatório para o login.")]
        [EmailAddress(ErrorMessage = "O e-mail informado é inválido.")]
        [StringLength(100, ErrorMessage ="O tamanho máximo do e-mail é inválido. Máximo {1} caracteres.")]
        public string Email { get; set; }
    }
}