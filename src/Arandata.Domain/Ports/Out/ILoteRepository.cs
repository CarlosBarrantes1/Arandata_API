using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;

namespace Arandata.Domain.Ports.Out
{
    public interface ILoteRepository
    {
        Task<IEnumerable<Lote>> GetAllAsync();
        Task<Lote?> GetByIdAsync(int id);
        Task AddAsync(Lote entity);
        Task UpdateAsync(Lote entity);
        Task DeleteAsync(Lote entity);
    }
}
