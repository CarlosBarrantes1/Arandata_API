using AutoMapper;
// Book and Loan DTO namespaces removed — agricultural DTOs only
using Arandata.Application.DTOs.Variedad;
using Arandata.Application.DTOs.Lote;
using Arandata.Application.DTOs.Cosecha;
using Arandata.Application.DTOs.Muestra100;
using Arandata.Application.DTOs.Baya100;
using Arandata.Application.DTOs.MuestraBrix;
using Arandata.Application.DTOs.BayaBrix;
using Arandata.Domain.Entities;

namespace Arandata.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book and Loan mappings removed

            // Agricultura mappings
            CreateMap<Variedad, VariedadDto>();
            CreateMap<CreateVariedadDto, Variedad>();
            CreateMap<UpdateVariedadDto, Variedad>();

            CreateMap<Lote, LoteDto>();
            CreateMap<CreateLoteDto, Lote>();
            CreateMap<UpdateLoteDto, Lote>();

            CreateMap<Cosecha, CosechaDto>();
            CreateMap<CreateCosechaDto, Cosecha>();
            CreateMap<UpdateCosechaDto, Cosecha>();

            CreateMap<Muestra100, Muestra100Dto>();
            CreateMap<CreateMuestra100Dto, Muestra100>();
            CreateMap<UpdateMuestra100Dto, Muestra100>();

            CreateMap<Baya100, Baya100Dto>();
            CreateMap<CreateBaya100Dto, Baya100>();
            CreateMap<UpdateBaya100Dto, Baya100>();

            CreateMap<MuestraBrix, MuestraBrixDto>();
            CreateMap<CreateMuestraBrixDto, MuestraBrix>();
            CreateMap<UpdateMuestraBrixDto, MuestraBrix>();

            CreateMap<BayaBrix, BayaBrixDto>();
            CreateMap<CreateBayaBrixDto, BayaBrix>();
            CreateMap<UpdateBayaBrixDto, BayaBrix>();
        }
    }
}
