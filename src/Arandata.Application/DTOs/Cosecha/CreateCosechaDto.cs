using System;

namespace Arandata.Application.DTOs.Cosecha
{
    public class CreateCosechaDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id_lote")]
        public int LoteId { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("fecha_cosecha")]
        public DateTime FechaCosecha { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("kg_total")]
        public decimal? KgTotal { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("kg_total_acumulado")]
        public decimal? KgTotalAcumulado { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("kg_planta")]
        public decimal? KgPlanta { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("kg_planta_acumulado")]
        public decimal? KgPlantaAcumulado { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("dias_despues_poda")]
        public int? DiasDespuesPoda { get; set; }
    }
}
