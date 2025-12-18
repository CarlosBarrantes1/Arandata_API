using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.Lote;

namespace Arandata.Application.Interfaces
{
    public interface ILoteService
    {
        Task<IEnumerable<LoteDto>> GetAllAsync();
        Task<LoteDto?> GetByIdAsync(int id);
        Task<LoteDto> CreateAsync(CreateLoteDto dto);
        Task UpdateAsync(int id, UpdateLoteDto dto);
        Task DeleteAsync(int id);
    }
}
