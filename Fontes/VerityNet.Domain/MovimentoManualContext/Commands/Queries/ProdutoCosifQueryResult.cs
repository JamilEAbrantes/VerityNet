using VerityNet.Domain.MovimentoManualContext.Enums;

namespace VerityNet.Domain.MovimentoManualContext.Commands.Queries
{
    public class ProdutoCosifQueryResult
    {
        public string Cod_Cosif { get; set; }
        public string Cod_Classificacao { get; set; }
        public EStatus Sta_Produto { get; set; }
        public string Cod_Produto { get; set; }
        public string Des_Produto { get; set; }
        public EStatus Sta_Cosif { get; set; }
    }
}
