using System;
using System.Linq;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Data.Implementations;
using Api.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Api.Data.Test
{
    public class UsuarioCrudCompleto : BaseTest, IClassFixture<DbTest>
    {
        private ServiceProvider _serviceProvider;

        public UsuarioCrudCompleto(DbTest dbTest)
        {
            _serviceProvider = dbTest.ServiceProvider;            
        }

        [Fact(DisplayName ="CRUD de Usuário")]
        [Trait("CRUD","UserEntity")]
        public async Task E_Possivel_Realizar_CRUD_usuario() {
            using (var context = _serviceProvider.GetService<MyContext>())
            {
                
                UserImplementation _repositorio = new UserImplementation(context);

                UserEntity _entity = new UserEntity
                {
                    Email = Faker.Internet.Email(),
                    Name = Faker.Name.FullName()     
                };

                // Teste de Inclusão C
                var _registroCriado = await _repositorio.InsertAsync(_entity);
                Assert.NotNull(_registroCriado);
                Assert.Equal(_entity.Email, _registroCriado.Email);
                Assert.Equal(_entity.Name, _registroCriado.Name);
                Assert.False(_registroCriado.ID ==  Guid.Empty);


                //Teste UPDATE U
                _entity.Name = Faker.Name.First();
                var _registroAtualizado = await _repositorio.UpdateAsync(_entity);
                Assert.NotNull(_registroAtualizado);
                Assert.Equal(_entity.Name, _registroAtualizado.Name);
                Assert.Equal(_entity.Email, _registroAtualizado.Email);

                //Teste Existe ID
                var _registroExiste = await _repositorio.ExistAsync(_registroAtualizado.ID);
                Assert.True(_registroExiste); 

                //Teste select com ID
                var _registroSelecionado = await _repositorio.SelectAsync(_registroAtualizado.ID);
                Assert.NotNull(_registroSelecionado);
                Assert.Equal(_registroAtualizado.Name, _registroSelecionado.Name);
                Assert.Equal(_registroAtualizado.Email, _registroSelecionado.Email);

                //Teste Select todos registros
                var _todosRegistros = await _repositorio.SelectAsync();
                Assert.NotNull(_todosRegistros);
                Assert.True(_todosRegistros.Count() > 0);

                // Teste de Exlcusão 
                var _removeu = await _repositorio.DeleteAsync(_registroSelecionado.ID);
                Assert.True(_removeu);


                // Teste de Login 
                var _usuarioPadrao = await _repositorio.FindByLogin("admin@admin.com");
                Assert.NotNull(_usuarioPadrao);
                Assert.Equal("admin@admin.com",_usuarioPadrao.Email);
                Assert.Equal("Administrador",_usuarioPadrao.Name);


            }
        }



    }
}