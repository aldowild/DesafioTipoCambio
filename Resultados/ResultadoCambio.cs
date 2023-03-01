namespace Desafio1.Resultado
{
    public class ResultadoCambio
    {
        public int CodigoResultado { get; set; }  // 0: Error, 1: Correcto
        public double Monto { get; set; }
        public double MontoConCambio { get; set; }
        public string MonedaOrigen { get; set; }
        public string MonedaDestino { get; set; }
        public double TipoCambio { get; set; }

    }
}
