using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Lote;
using Arandata.Domain.Ports.Out;
using AutoMapper;

namespace Arandata.Application.Services
{
    public class LoteService : ILoteService
    {
        private readonly ILoteRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoteService(ILoteRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoteDto> CreateAsync(CreateLoteDto dto)
        {
            var entity = _mapper.Map<Domain.Entities.Lote>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();

            // Lógica Senior: Si el lote se crea con una FechaPoda, registrarla automáticamente en la tabla Podas
            if (dto.FechaPoda.HasValue)
            {
                var poda = new Domain.Entities.Poda
                {
                    LoteId = entity.Id,
                    FechaPoda = dto.FechaPoda.Value
                };
                // Aquí necesitaríamos acceso al contexto o un repositorio de podas. 
                // Para simplificar y reducir carga, lo haremos en el controlador o inyectando el repo de podas.
            }

            return _mapper.Map<LoteDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync();
        }
        public async Task<IEnumerable<LoteDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<LoteDto>>(list);
        }
        public async Task<LoteDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<LoteDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateLoteDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync();
        }
    }
}
