using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;
using Arandata.Domain.Ports.Out;
using Arandata.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Arandata.Infrastructure.Persistence.Repositories
{
    public class BayaBrixRepository : IBayaBrixRepository
    {
        private readonly ApplicationDbContext _context;
        public BayaBrixRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(BayaBrix entity)
        {
            await _context.BayasBrix.AddAsync(entity);
        }

        public async Task DeleteAsync(BayaBrix entity)
        {
            _context.BayasBrix.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<BayaBrix>> GetAllAsync()
        {
            return await _context.BayasBrix.AsNoTracking().ToListAsync();
        }

        public async Task<BayaBrix?> GetByIdAsync(int id)
        {
            return await _context.BayasBrix.FindAsync(id);
        }

        public async Task UpdateAsync(BayaBrix entity)
        {
            _context.BayasBrix.Update(entity);
            await Task.CompletedTask;
        }
    }
}
