namespace Arandata.Domain.Entities
{
    public class BayaBrix
    {
        public int Id { get; set; }
        public int MuestraBrixId { get; set; }
        public MuestraBrix? MuestraBrix { get; set; }
        public int NumeroBaya { get; set; }
        public decimal? BrixBaya { get; set; }
    }
}
