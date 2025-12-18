using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Muestra100;

namespace Arandata.Application.Services
{
    public class Muestra100Service : IMuestra100Service
    {
        public Task<Muestra100Dto> CreateAsync(CreateMuestra100Dto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<Muestra100Dto>> GetAllAsync() => throw new NotImplementedException();
        public Task<Muestra100Dto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateMuestra100Dto dto) => throw new NotImplementedException();
    }
}
