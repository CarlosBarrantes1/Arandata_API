using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.MuestraBrix;

namespace Arandata.Application.Services
{
    public class MuestraBrixService : IMuestraBrixService
    {
        public Task<MuestraBrixDto> CreateAsync(CreateMuestraBrixDto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<MuestraBrixDto>> GetAllAsync() => throw new NotImplementedException();
        public Task<MuestraBrixDto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateMuestraBrixDto dto) => throw new NotImplementedException();
    }
}
