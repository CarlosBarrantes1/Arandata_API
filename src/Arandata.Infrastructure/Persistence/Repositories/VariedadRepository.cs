using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;
using Arandata.Domain.Ports.Out;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Arandata.Infrastructure.Persistence.Repositories
{
    public class VariedadRepository : IVariedadRepository
    {
        private readonly ApplicationDbContext _context;
        public VariedadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Variedad entity)
        {
            await _context.Variedades.AddAsync(entity);
        }

        public async Task DeleteAsync(Variedad entity)
        {
            _context.Variedades.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Variedad>> GetAllAsync()
        {
            return await _context.Variedades.AsNoTracking().ToListAsync();
        }

        public async Task<Variedad?> GetByIdAsync(int id)
        {
            return await _context.Variedades.FindAsync(id);
        }

        public async Task UpdateAsync(Variedad entity)
        {
            _context.Variedades.Update(entity);
            await Task.CompletedTask;
        }
    }
}
