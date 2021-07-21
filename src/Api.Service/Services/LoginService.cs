using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Api.Domain.Dtos;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Repository;
using Api.Domain.Secuity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Service.Services
{
    public class LoginService : ILoginService
    {
        private IUserRepository _repository;
        private SigningConfiguration _signingConfiguration;
        private TokenConfiguration _tokenConfiguration;

        private IConfiguration _configuration;

        public LoginService(
            IUserRepository repository,
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration,
            IConfiguration configuration)
        {
            // Injeção de Dependencia 
            _repository = repository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
            _configuration = configuration;
        }

        public async Task<object> FindByLogin(LoginDto user)
        {
            var baseUser = new UserEntity();

            if (user != null && !string.IsNullOrWhiteSpace(user.Email))
            {

                baseUser = await _repository.FindByLogin(user.Email);

                if (baseUser == null ){
                    return new {
                        authenticate = false,
                        message = "Falha ao autenticar"
                    };
                } else {
                    //Inicio da FOrmação do JWT
                    ClaimsIdentity identity = new ClaimsIdentity(
                        new GenericIdentity(user.Email),
                        new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),

                        }
                    );

                    DateTime createDate = DateTime.Now;
                    DateTime expirationDate = createDate + TimeSpan.FromSeconds(_tokenConfiguration.Seconds);

                    var handle = new JwtSecurityTokenHandler();

                    string token = CreateToken(identity,createDate,expirationDate,handle);

                    return SuccessObject(createDate,expirationDate,token,user);
                } 

            } else {
                return null;
            }
        }

        //Metodo que cria a o Token, para ser utilizado na chamada FindById;
        private string CreateToken(
            ClaimsIdentity identity,
            DateTime createData,
            DateTime expirationData,
            JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken( new SecurityTokenDescriptor
            {
               Issuer = _tokenConfiguration.Issuer,
               Audience = _tokenConfiguration.Audience,
               SigningCredentials = _signingConfiguration.SigningCredentials,
               Subject = identity,
               NotBefore = createData,
               Expires = expirationData     
            });

           var token = handler.WriteToken(securityToken);

           return token; 

        
        }

        
        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, LoginDto user)
        {
            return new 
            {
                authenticated = true,
                created = createDate.ToString("yyyy-MMM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MMM-dd HH:mm:ss"),
                accessToken = token,
                userName = user.Email,
                message = "Usuário logado com sucesso."
            };
        }

    }
}