namespace Arandata.Domain.Entities
{
    public class BayaPeso
    {
        public int Id { get; set; }
        public int MuestraId { get; set; }
        public int NumeroBaya { get; set; }
        public decimal Peso { get; set; }

        public Muestra? Muestra { get; set; }
    }
}
