using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Variedad;
using Arandata.Domain.Entities;
using Arandata.Domain.Ports.Out;
using AutoMapper;

namespace Arandata.Application.Services
{
    public class VariedadService : IVariedadService
    {
        private readonly IVariedadRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VariedadService(IVariedadRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VariedadDto> CreateAsync(CreateVariedadDto dto)
        {
            var entity = _mapper.Map<Variedad>(dto);
            await _repository.AddAsync(entity);
            _unitOfWork.Commit();
            return _mapper.Map<VariedadDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            await _repository.DeleteAsync(entity);
            _unitOfWork.Commit();
        }

        public async Task<IEnumerable<VariedadDto>> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<VariedadDto>>(list);
        }

        public async Task<VariedadDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<VariedadDto>(entity);
        }

        public async Task UpdateAsync(int id, UpdateVariedadDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            _unitOfWork.Commit();
        }
    }
}
