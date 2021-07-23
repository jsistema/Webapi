using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Dtos.User;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Api.Domain.Interfaces.Services.User;
using Api.Domain.Models;
using AutoMapper;

namespace Api.Service.Services
{
    public class UserService : IUserService
    {
        
        private IRepository<UserEntity> _repository;
        private readonly IMapper _mapper;

        public UserService(IRepository<UserEntity> repository, IMapper mapper)
        {
           //Passa para o local, a referencia ao repositório
           // Injeção de Dependência   
          _repository = repository;  
          _mapper = mapper;
        }

        public async Task<bool> Delete(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        public async Task<UserDto> Get(Guid id)
        {
            var entity = await _repository.SelectAsync(id);
            return _mapper.Map<UserDto>(entity);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var listEntity = await _repository.SelectAsync();
            return _mapper.Map<IEnumerable<UserDto>>(listEntity);
        }

        public async Task<UserDtoCreateResult> Post(UserDtoCreate user)
        {

          //Converte DTO para um Modelo   
          var model = _mapper.Map<UserModel>(user); 

         //Converte o Modelo para uma Entidade
          var entity = _mapper.Map<UserEntity>(model); 

         //Insere no Banco, baseado na entidade com o EF   
          var result = await _repository.InsertAsync(entity);

         //Retornar o a entidade de resultado convertida para um UserDTOCreateResult;   
          return _mapper.Map<UserDtoCreateResult>(result); 

        }

        public async Task<UserDtoUpdateResult> Put(UserDtoUpdate user)
        {

            var model = _mapper.Map<UserModel>(user);

            var entity = _mapper.Map<UserEntity>(model);

            var result = await _repository.UpdateAsync(entity);

            return _mapper.Map<UserDtoUpdateResult>(result);
        }
    }
}