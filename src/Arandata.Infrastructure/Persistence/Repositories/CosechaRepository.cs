using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;
using Arandata.Domain.Ports.Out;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Arandata.Infrastructure.Persistence.Repositories
{
    public class CosechaRepository : ICosechaRepository
    {
        private readonly ApplicationDbContext _context;
        public CosechaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Cosecha entity)
        {
            await _context.Cosechas.AddAsync(entity);
        }

        public async Task DeleteAsync(Cosecha entity)
        {
            _context.Cosechas.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Cosecha>> GetAllAsync()
        {
            return await _context.Cosechas
                .AsNoTracking()
                .Include(c => c.Lote)
                .ThenInclude(l => l.Variedad)
                .ToListAsync();
        }

        public async Task<Cosecha?> GetByIdAsync(int id)
        {
            return await _context.Cosechas.FindAsync(id);
        }

        public async Task UpdateAsync(Cosecha entity)
        {
            _context.Cosechas.Update(entity);
            await Task.CompletedTask;
        }
    }
}
