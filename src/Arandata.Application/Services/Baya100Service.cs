using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Baya100;

namespace Arandata.Application.Services
{
    public class Baya100Service : IBaya100Service
    {
        public Task<Baya100Dto> CreateAsync(CreateBaya100Dto dto) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
        public Task<IEnumerable<Baya100Dto>> GetAllAsync() => throw new NotImplementedException();
        public Task<Baya100Dto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateBaya100Dto dto) => throw new NotImplementedException();
    }
}
