using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Cosecha;

namespace Arandata.Application.Services
{
    public class CosechaService : ICosechaService
    {
        public Task<CosechaDto> CreateAsync(CreateCosechaDto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<CosechaDto>> GetAllAsync() => throw new NotImplementedException();
        public Task<CosechaDto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateCosechaDto dto) => throw new NotImplementedException();
    }
}
