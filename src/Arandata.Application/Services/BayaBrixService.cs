using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.BayaBrix;

namespace Arandata.Application.Services
{
    public class BayaBrixService : IBayaBrixService
    {
        public Task<BayaBrixDto> CreateAsync(CreateBayaBrixDto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<BayaBrixDto>> GetAllAsync() => throw new NotImplementedException();
        public Task<BayaBrixDto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateBayaBrixDto dto) => throw new NotImplementedException();
    }
}
