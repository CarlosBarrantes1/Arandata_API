namespace Arandata.Domain.Entities
{
    public class BayaDiametro
    {
        public int Id { get; set; }
        public int MuestraId { get; set; }
        public int NumeroBaya { get; set; }
        public decimal Diametro { get; set; }

        public Muestra? Muestra { get; set; }
    }
}
