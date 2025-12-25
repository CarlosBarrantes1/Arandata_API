using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;
using Arandata.Domain.Ports.Out;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Arandata.Infrastructure.Persistence.Repositories
{
    public class LoteRepository : ILoteRepository
    {
        private readonly ApplicationDbContext _context;
        public LoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Lote entity)
        {
            await _context.Lotes.AddAsync(entity);
        }

        public async Task DeleteAsync(Lote entity)
        {
            _context.Lotes.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Lote>> GetAllAsync()
        {
            return await _context.Lotes.AsNoTracking().ToListAsync();
        }

        public async Task<Lote?> GetByIdAsync(int id)
        {
            return await _context.Lotes.FindAsync(id);
        }

        public async Task UpdateAsync(Lote entity)
        {
            _context.Lotes.Update(entity);
            await Task.CompletedTask;
        }
    }
}
