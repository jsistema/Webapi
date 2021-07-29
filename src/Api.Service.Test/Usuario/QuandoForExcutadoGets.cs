using System;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Interfaces.Services.User;
using Moq;
using Xunit;

namespace Api.Service.Test.Usuario
{
    public class QuandoForExcutadoGets : UsuarioTestes
    {

        private IUserService _service;

        private Mock<IUserService> _serviceMock;

        [Fact(DisplayName = "É possível executar o metódo get")]

        public async Task EPossivelExecutarMetodoGet()
        {
            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Get(IdUsuario)).ReturnsAsync(userDto);
            _service = _serviceMock.Object;

            // Verifica o get com ID    
            var result = await _service.Get(IdUsuario);
            Assert.NotNull(result);
            Assert.True(result.Id==IdUsuario);
            Assert.Equal(NomeUsuario,result.Name);

            _serviceMock = new Mock<IUserService>();
            _serviceMock.Setup(m => m.Get(It.IsAny<Guid>())).Returns(Task.FromResult((UserDto)null));
            _service = _serviceMock.Object;   

            var _record = await _service.Get(IdUsuario);
            Assert.Null(_record);

        }


    }
}