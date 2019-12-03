using System.Collections.Generic;
using VerityNet.Domain.MovimentoManualContext.Commands.Queries;
using VerityNet.Domain.MovimentoManualContext.Entities;
using VerityNet.Domain.MovimentoManualContext.Enums;

namespace VerityNet.Domain.MovimentoManualContext.Repositories
{
    public interface IMovimentoManualRepository
    {
        MovimentoManualQueryResult MovimentoManual(decimal datMes, decimal datAno, decimal numLancemanto, string codProduto, string codCosif);
        IEnumerable<MovimentoManualQueryResult> ListarMovimentosManuais();        
        void Salvar(MovimentoManual movimentoManual);
        Produto Produto(string codProduto, EStatus status);
        IEnumerable<ProdutoQueryResult> ListarProdutos();
        ProdutoCosif ProdutoCosif(string codCosif, EStatus status);
        IEnumerable<ProdutoCosifQueryResult> ListarProdutosCosifs();
        int ObterProximoLancamento(decimal datMes, decimal datAno, string codProduto, string codCosif);
    }
}