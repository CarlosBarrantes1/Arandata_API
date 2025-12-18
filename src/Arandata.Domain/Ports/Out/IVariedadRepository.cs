using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;

namespace Arandata.Domain.Ports.Out
{
    public interface IVariedadRepository
    {
        Task<IEnumerable<Variedad>> GetAllAsync();
        Task<Variedad?> GetByIdAsync(int id);
        Task AddAsync(Variedad entity);
        Task UpdateAsync(Variedad entity);
        Task DeleteAsync(Variedad entity);
    }
}
