using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.Baya100;

namespace Arandata.Application.Interfaces
{
    public interface IBaya100Service
    {
        Task<IEnumerable<Baya100Dto>> GetAllAsync();
        Task<Baya100Dto?> GetByIdAsync(int id);
        Task<Baya100Dto> CreateAsync(CreateBaya100Dto dto);
        Task UpdateAsync(int id, UpdateBaya100Dto dto);
        Task DeleteAsync(int id);
    }
}
