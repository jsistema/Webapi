using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.Usuario
{
    public class QuandoForExcutadoUpdate : UsuarioTestes
    {
        private IUserService _service;

        private Mock<IUserService> _serviceMock;   

        [Fact(DisplayName = "É possível executar o metódo Created POST")]
        public async Task EPossivelExecutarMetodoUpdate()
        {
            // para testar um update, primeiro faz um post/created 
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Post(userDtoCreate)).ReturnsAsync(UserDtoCreateResult);
            _service = _serviceMock.Object;

            var result = await _service.Post(userDtoCreate);
            Assert.NotNull(result);
            Assert.Equal(NomeUsuario,result.Name);
            Assert.Equal(EmailUsuario,result.Email);

            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Put(userDtoUpdate)).ReturnsAsync(userDtoUpdateResult);
            _service = _serviceMock.Object;

            var resultUpdate = await _service.Put(userDtoUpdate);
            Assert.NotNull(resultUpdate);
            Assert.Equal(NomeUsuarioAlterado,resultUpdate.Name);
            Assert.Equal(EmailUsuarioAlterado,resultUpdate.Email);


        }   
    }
}