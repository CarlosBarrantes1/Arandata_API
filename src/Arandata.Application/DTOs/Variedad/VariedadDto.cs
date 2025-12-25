// ...existing code...
namespace Arandata.Application.DTOs.Variedad
{
    public class VariedadDto
    {
        [System.Text.Json.Serialization.JsonPropertyName("id_variedad")]
        public int IdVariedad { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;
        [System.Text.Json.Serialization.JsonPropertyName("densidad_plantas")]
        public int DensidadPlantas { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("plantas_por_variedad")]
        public int? PlantasPorVariedad { get; set; }
    }
}
