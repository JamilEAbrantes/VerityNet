namespace VerityNet.Domain.MovimentoManualContext.Commands.Queries
{
    public class MovimentoManualQueryResult
    {
        public decimal Dat_Mes { get; set; }
        public decimal Dat_Ano { get; set; }
        public string Cod_Produto { get; set; }
        public string Des_Produto { get; set; }
        public decimal Num_Lancemanto { get; set; }
        public string Des_Descricao { get; set; }
        public decimal Val_Valor { get; set; }
    }
}
