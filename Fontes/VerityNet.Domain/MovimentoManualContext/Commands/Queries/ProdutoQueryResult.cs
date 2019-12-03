using VerityNet.Domain.MovimentoManualContext.Enums;

namespace VerityNet.Domain.MovimentoManualContext.Commands.Queries
{
    public class ProdutoQueryResult
    {
        public string Cod_Produto { get; set; }
        public string Des_Produto { get; set; }
        public EStatus Sta_Status { get; set; }
    }
}
