namespace Arandata.Domain.Entities
{
    public enum TipoMuestra
    {
        DIAMETRO,
        PESO,
        BRIX
    }

    public class MuestraTipo
    {
        public int Id { get; set; }
        public int MuestraId { get; set; }
        public Muestra? Muestra { get; set; }
        public TipoMuestra Tipo { get; set; }
        public int Cantidad { get; set; }
    }
}
