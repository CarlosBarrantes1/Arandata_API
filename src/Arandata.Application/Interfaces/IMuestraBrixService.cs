using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.DTOs.MuestraBrix;

namespace Arandata.Application.Interfaces
{
    public interface IMuestraBrixService
    {
        Task<IEnumerable<MuestraBrixDto>> GetAllAsync();
        Task<MuestraBrixDto?> GetByIdAsync(int id);
        Task<MuestraBrixDto> CreateAsync(CreateMuestraBrixDto dto);
        Task UpdateAsync(int id, UpdateMuestraBrixDto dto);
        Task DeleteAsync(int id);
    }
}
