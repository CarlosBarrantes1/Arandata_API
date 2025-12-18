namespace Arandata.Application.DTOs.Variedad
{
    public class CreateVariedadDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int DensidadPlantas { get; set; }
        public int? PlantasPorVariedad { get; set; }
    }
}
