using AutoMapper;
// Book and Loan DTO namespaces removed — agricultural DTOs only
using Arandata.Application.DTOs.Variedad;
using Arandata.Application.DTOs.Lote;
using Arandata.Application.DTOs.Cosecha;
using Arandata.Domain.Entities;

namespace Arandata.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book and Loan mappings removed

            // Agricultura mappings
            CreateMap<Variedad, VariedadDto>()
                .ForMember(dest => dest.IdVariedad, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.DensidadPlantas, opt => opt.MapFrom(src => src.DensidadPlantas))
                .ForMember(dest => dest.PlantasPorVariedad, opt => opt.MapFrom(src => src.PlantasPorVariedad));
            CreateMap<CreateVariedadDto, Variedad>();
            CreateMap<UpdateVariedadDto, Variedad>();

            CreateMap<Lote, LoteDto>()
                .ForMember(dest => dest.idLote, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.variedadId, opt => opt.MapFrom(src => src.VariedadId));
            CreateMap<CreateLoteDto, Lote>()
                .ForMember(dest => dest.VariedadId, opt => opt.MapFrom(src => src.VariedadId));
            CreateMap<UpdateLoteDto, Lote>()
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.VariedadId, opt => opt.MapFrom(src => src.variedadId))
                .ForMember(dest => dest.FechaSiembra, opt => opt.MapFrom(src => src.FechaSiembra))
                .ForMember(dest => dest.PlantasTotales, opt => opt.MapFrom(src => src.PlantasTotales))
                .ForMember(dest => dest.Hectareas, opt => opt.MapFrom(src => src.Hectareas));

            CreateMap<Cosecha, CosechaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LoteId, opt => opt.MapFrom(src => src.LoteId))
                .ForMember(dest => dest.NombreVariedad, opt => opt.MapFrom(src => src.Lote != null && src.Lote.Variedad != null ? src.Lote.Variedad.Nombre : null));
            CreateMap<CreateCosechaDto, Cosecha>()
                .ForMember(dest => dest.LoteId, opt => opt.MapFrom(src => src.LoteId));
            CreateMap<UpdateCosechaDto, Cosecha>();


        }
    }
}
