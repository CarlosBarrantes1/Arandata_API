using System;

namespace Arandata.Application.DTOs.Lote
{
    public class LoteDto
    {
        public int IdLote { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdVariedad { get; set; }
        public DateTime? FechaSiembra { get; set; }
        public DateTime? FechaPoda { get; set; }
        public int? PlantasTotales { get; set; }
        public decimal? Hectareas { get; set; }
    }
}
