using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;

namespace Arandata.Domain.Ports.Out
{
    public interface ICosechaRepository
    {
        Task<IEnumerable<Cosecha>> GetAllAsync();
        Task<Cosecha?> GetByIdAsync(int id);
        Task AddAsync(Cosecha entity);
        Task UpdateAsync(Cosecha entity);
        Task DeleteAsync(Cosecha entity);
    }
}
