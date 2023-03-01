using System.ComponentModel.DataAnnotations;

namespace Desafio1.DataModel
{
    public class CambioDataModel
    {
        [Key]
        public Guid Id { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public double TipoCambio { get; set; }
    }
}
