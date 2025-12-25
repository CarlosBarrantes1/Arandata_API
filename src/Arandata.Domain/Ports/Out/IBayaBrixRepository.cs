using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Domain.Entities;

namespace Arandata.Domain.Ports.Out
{
    public interface IBayaBrixRepository
    {
        Task<IEnumerable<BayaBrix>> GetAllAsync();
        Task<BayaBrix?> GetByIdAsync(int id);
        Task AddAsync(BayaBrix entity);
        Task UpdateAsync(BayaBrix entity);
        Task DeleteAsync(BayaBrix entity);
    }
}
