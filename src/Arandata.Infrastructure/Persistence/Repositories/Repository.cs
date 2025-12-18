using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arandata.Infrastructure.Persistence.Repositories
{
    public class Repository<T> where T : class
    {
        protected readonly DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
