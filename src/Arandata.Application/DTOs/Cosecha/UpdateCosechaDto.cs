using System;

namespace Arandata.Application.DTOs.Cosecha
{
    public class UpdateCosechaDto
    {
        public DateTime FechaCosecha { get; set; }
        public decimal? KgTotal { get; set; }
        public decimal? KgTotalAcumulado { get; set; }
        public decimal? KgPlanta { get; set; }
        public decimal? KgPlantaAcumulado { get; set; }
        public int? DiasDespuesPoda { get; set; }
    }
}
