using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.Variedad;

namespace Arandata.Application.Interfaces
{
    public interface IVariedadService
    {
        Task<IEnumerable<VariedadDto>> GetAllAsync();
        Task<VariedadDto?> GetByIdAsync(int id);
        Task<VariedadDto> CreateAsync(CreateVariedadDto dto);
        Task UpdateAsync(int id, UpdateVariedadDto dto);
        Task DeleteAsync(int id);
    }
}
