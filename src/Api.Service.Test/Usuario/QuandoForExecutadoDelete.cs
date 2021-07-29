using System;
using System.Threading.Tasks;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.Usuario
{
    public class QuandoForExecutadoDelete : UsuarioTestes
    {


        private IUserService _service;

        private Mock<IUserService> _serviceMock;   

        [Fact(DisplayName = "É possível executar o metódo Delete")]
        public async Task EPossivelExecutarMetodoDelete()
        {
            // para testar um update, primeiro faz um post/created 
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Delete(It.IsAny<Guid>())).ReturnsAsync(true);
            _service = _serviceMock.Object;

            var _deletado = await _service.Delete(IdUsuario);
            Assert.True(_deletado);



        }   
        
    }
}