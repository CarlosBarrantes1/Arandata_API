using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Lote;

namespace Arandata.Application.Services
{
    public class LoteService : ILoteService
    {
        public Task<LoteDto> CreateAsync(CreateLoteDto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<LoteDto>> GetAllAsync() => throw new NotImplementedException();
        public Task<LoteDto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateLoteDto dto) => throw new NotImplementedException();
    }
}
