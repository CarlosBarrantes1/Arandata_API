namespace Arandata.Domain.Entities
{
    public class Variedad
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int DensidadPlantas { get; set; }
        public int? PlantasPorVariedad { get; set; }

        public ICollection<Lote> Lotes { get; set; } = new List<Lote>();
    }
}
