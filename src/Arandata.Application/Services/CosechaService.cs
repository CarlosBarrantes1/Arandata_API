using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arandata.Application.Interfaces;
using Arandata.Application.DTOs.Cosecha;
using Arandata.Domain.Ports.Out;

namespace Arandata.Application.Services
{
    public class CosechaService : ICosechaService
    {
        private readonly ICosechaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AutoMapper.IMapper _mapper;

        public CosechaService(ICosechaRepository repository, IUnitOfWork unitOfWork, AutoMapper.IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CosechaDto> CreateAsync(CreateCosechaDto dto)
        {
            var entity = _mapper.Map<Arandata.Domain.Entities.Cosecha>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            return _mapper.Map<CosechaDto>(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;
            await _repository.DeleteAsync(entity);
            await _unitOfWork.CommitAsync();
        }
        public async Task<IEnumerable<CosechaDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CosechaDto>>(entities);
        }
        public Task<CosechaDto?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, UpdateCosechaDto dto) => throw new NotImplementedException();
    }
}
