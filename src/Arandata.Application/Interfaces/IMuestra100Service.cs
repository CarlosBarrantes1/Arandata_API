using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.Muestra100;

namespace Arandata.Application.Interfaces
{
    public interface IMuestra100Service
    {
        Task<IEnumerable<Muestra100Dto>> GetAllAsync();
        Task<Muestra100Dto?> GetByIdAsync(int id);
        Task<Muestra100Dto> CreateAsync(CreateMuestra100Dto dto);
        Task UpdateAsync(int id, UpdateMuestra100Dto dto);
        Task DeleteAsync(int id);
    }
}
