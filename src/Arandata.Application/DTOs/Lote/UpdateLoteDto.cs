using System;

namespace Arandata.Application.DTOs.Lote
{
    public class UpdateLoteDto
    {
        public string Nombre { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonPropertyName("variedadId")]
        public int variedadId { get; set; }
        public DateTime? FechaSiembra { get; set; }
        public DateTime? FechaPoda { get; set; }
        public int? PlantasTotales { get; set; }
        public decimal? Hectareas { get; set; }
    }
}
