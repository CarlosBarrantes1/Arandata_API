using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.Cosecha;

namespace Arandata.Application.Interfaces
{
    public interface ICosechaService
    {
        Task<IEnumerable<CosechaDto>> GetAllAsync();
        Task<CosechaDto?> GetByIdAsync(int id);
        Task<CosechaDto> CreateAsync(CreateCosechaDto dto);
        Task UpdateAsync(int id, UpdateCosechaDto dto);
        Task DeleteAsync(int id);
    }
}
