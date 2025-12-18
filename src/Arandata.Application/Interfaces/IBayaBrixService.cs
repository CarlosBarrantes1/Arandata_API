using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.BayaBrix;

namespace Arandata.Application.Interfaces
{
    public interface IBayaBrixService
    {
        Task<IEnumerable<BayaBrixDto>> GetAllAsync();
        Task<BayaBrixDto?> GetByIdAsync(int id);
        Task<BayaBrixDto> CreateAsync(CreateBayaBrixDto dto);
        Task UpdateAsync(int id, UpdateBayaBrixDto dto);
        Task DeleteAsync(int id);
    }
}
