namespace Arandata.Domain.Entities
{
    public class Baya100
    {
        public int Id { get; set; }
        public int Muestra100Id { get; set; }
        public Muestra100? Muestra100 { get; set; }
        public int NumeroBaya { get; set; }
        public decimal? PesoBaya { get; set; }
        public decimal? DiametroBaya { get; set; }
    }
}
