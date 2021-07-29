using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Domain.Dtos.Municipio;

namespace Api.Domain.Interfaces.Services.Municipio
{
    public interface IMunicipioService
    {
        Task<MunicipioDto> Get(Guid id);

        Task<MunicipioDtoCompleto> GetCompleteById(Guid id);

        Task<MunicipioDtoCompleto> GetCompleteByIBGE(int IBGE);

        Task<IEnumerable<MunicipioDto>> GetAll();

        Task<MunicipioDtoCreate> Post(MunicipioDtoCreate municipio);

        Task<MunicipioDtoUpdate> Put(MunicipioDtoUpdate municipio);    
        
         Task<bool> Delete(Guid id);
    }
}