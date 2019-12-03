using System.Collections.Generic;
using System.Data;
using VerityNet.Domain.MovimentoManualContext.Commands.Queries;
using VerityNet.Domain.MovimentoManualContext.Entities;
using VerityNet.Domain.MovimentoManualContext.Enums;
using VerityNet.Domain.MovimentoManualContext.Repositories;
using VerityNet.Infra.Data.DataContext;

namespace VerityNet.Dao.MovimentoManualContext.Repositories
{
    public class MovimentoManualRepository : IMovimentoManualRepository
    {
        private readonly VerityNetDataContext _context;

        public MovimentoManualRepository(VerityNetDataContext context)
        {
            _context = context;
        }

        public MovimentoManualQueryResult MovimentoManual(
            decimal datMes,
            decimal datAno,
            decimal numLancemanto,
            string codProduto,
            string codCosif)
        {
            _context.AdicionarParametro("@Dat_Mes", SqlDbType.Decimal, datMes);
            _context.AdicionarParametro("@Dat_Ano", SqlDbType.Decimal, datAno);
            _context.AdicionarParametro("@Num_Lancemanto", SqlDbType.Decimal, numLancemanto);
            _context.AdicionarParametro("@Cod_Produto", SqlDbType.Char, codProduto);
            _context.AdicionarParametro("@Cod_Cosif", SqlDbType.Char, codCosif);

            var dataTable = _context.ExecutarConsulta("spObterMovimentoManual");
            if (dataTable.Rows.Count == 0)
                return null;

            var movimentoManualQueryResult = new MovimentoManualQueryResult();
            movimentoManualQueryResult.Dat_Mes = (decimal)dataTable.Rows[0]["Dat_Mes"];
            movimentoManualQueryResult.Dat_Ano = (decimal)dataTable.Rows[0]["Dat_Ano"];
            movimentoManualQueryResult.Cod_Produto = dataTable.Rows[0]["Cod_Produto"].ToString();
            movimentoManualQueryResult.Des_Produto = dataTable.Rows[0]["Des_Produto"].ToString();
            movimentoManualQueryResult.Num_Lancemanto = (decimal)dataTable.Rows[0]["Num_Lancemanto"];
            movimentoManualQueryResult.Des_Descricao = dataTable.Rows[0]["Des_Descricao"].ToString();
            movimentoManualQueryResult.Val_Valor = (decimal)dataTable.Rows[0]["Val_Valor"];

            return movimentoManualQueryResult;
        }

        public IEnumerable<MovimentoManualQueryResult> ListarMovimentosManuais()
        {
            var movimentosManuaisQueryResult = new List<MovimentoManualQueryResult>();

            var dataTable = _context.ExecutarConsulta("spListarMovimentosManuais");

            foreach (DataRow row in dataTable.Rows)
            {
                var movimentoManualQueryResult = new MovimentoManualQueryResult();
                movimentoManualQueryResult.Dat_Mes = (decimal)row["Dat_Mes"];
                movimentoManualQueryResult.Dat_Ano = (decimal)row["Dat_Ano"];
                movimentoManualQueryResult.Cod_Produto = row["Cod_Produto"].ToString();
                movimentoManualQueryResult.Des_Produto = row["Des_Produto"].ToString();
                movimentoManualQueryResult.Num_Lancemanto = (decimal)row["Num_Lancemanto"];
                movimentoManualQueryResult.Des_Descricao = row["Des_Descricao"].ToString();
                movimentoManualQueryResult.Val_Valor = (decimal)row["Val_Valor"];
                movimentosManuaisQueryResult.Add(movimentoManualQueryResult);
            }

            return movimentosManuaisQueryResult;
        }

        public void Salvar(MovimentoManual movimentoManual)
        {
            _context.AdicionarParametro("@Dat_Mes", SqlDbType.Decimal, movimentoManual.Dat_Mes);
            _context.AdicionarParametro("@Dat_Ano", SqlDbType.Decimal, movimentoManual.Dat_Ano);
            _context.AdicionarParametro("@Num_Lancemanto", SqlDbType.Decimal, movimentoManual.Num_Lancemanto);
            _context.AdicionarParametro("@Cod_Produto", SqlDbType.Char, movimentoManual.Produto.Cod_Produto);
            _context.AdicionarParametro("@Cod_Cosif", SqlDbType.Char, movimentoManual.ProdutoCosif.Cod_Cosif);
            _context.AdicionarParametro("@Val_Valor", SqlDbType.Decimal, movimentoManual.Val_Valor);
            _context.AdicionarParametro("@Des_Descricao", SqlDbType.VarChar, movimentoManual.Des_Descricao);
            _context.AdicionarParametro("@Dat_Movimento", SqlDbType.DateTime, movimentoManual.Dat_Movimento);
            _context.AdicionarParametro("@Cod_Usuario", SqlDbType.VarChar, movimentoManual.UsuarioVO.Cod_Usuario);

            _context.ExecutarCommando("spCriarMovimentoManual");
        }

        public Produto Produto(string codProduto, EStatus status)
        {
            _context.AdicionarParametro("@Cod_Produto", SqlDbType.Char, codProduto);
            _context.AdicionarParametro("@Sta_Status", SqlDbType.Char, status);

            var dataTable = _context.ExecutarConsulta("spObterProduto");
            if (dataTable.Rows.Count == 0)
                return null;

            var produto = new Produto(
                dataTable.Rows[0]["Cod_Produto"].ToString(),
                dataTable.Rows[0]["Des_Produto"].ToString(),
                dataTable.Rows[0]["Sta_Status"].ToString() == "A" ? EStatus.A : EStatus.I);

            return produto;
        }

        public IEnumerable<ProdutoQueryResult> ListarProdutos()
        {
            var produtosQueryResult = new List<ProdutoQueryResult>();

            var dataTable = _context.ExecutarConsulta("spListarProdutos");

            foreach (DataRow row in dataTable.Rows)
            {
                var produtoQueryResult = new ProdutoQueryResult();
                produtoQueryResult.Cod_Produto = row["Cod_Produto"].ToString();
                produtoQueryResult.Des_Produto = row["Des_Produto"].ToString();
                produtoQueryResult.Sta_Status = row["Sta_Status"].ToString() == "A" ? EStatus.A : EStatus.I;

                produtosQueryResult.Add(produtoQueryResult);
            }

            return produtosQueryResult;
        }

        public ProdutoCosif ProdutoCosif(
            string codCosif,
            EStatus status)
        {
            _context.AdicionarParametro("@Cod_Cosif", SqlDbType.Char, codCosif);
            _context.AdicionarParametro("@Sta_Status", SqlDbType.Char, status);

            var dataTable = _context.ExecutarConsulta("spListarProdutosCosif");
            if (dataTable.Rows.Count == 0)
                return null;

            var produto = new Produto(
                dataTable.Rows[0]["Cod_Produto"].ToString(),
                dataTable.Rows[0]["Des_Produto"].ToString(),
                dataTable.Rows[0]["Sta_Produto"].ToString() == "A" ? EStatus.A : EStatus.I);

            var produtoCosif = new ProdutoCosif(
                produto,
                dataTable.Rows[0]["Cod_Cosif"].ToString(),
                dataTable.Rows[0]["Cod_Classificacao"].ToString(),
                dataTable.Rows[0]["Sta_Cosif"].ToString() == "A" ? EStatus.A : EStatus.I);

            return produtoCosif;
        }

        public IEnumerable<ProdutoCosifQueryResult> ListarProdutosCosifs()
        {
            var produtosCosifsQueryResult = new List<ProdutoCosifQueryResult>();

            var dataTable = _context.ExecutarConsulta("spListarProdutosCosifs");

            foreach (DataRow row in dataTable.Rows)
            {
                var produtoCosifQueryResult = new ProdutoCosifQueryResult();
                produtoCosifQueryResult.Cod_Produto = row["Cod_Produto"].ToString();
                produtoCosifQueryResult.Des_Produto = row["Des_Produto"].ToString();
                produtoCosifQueryResult.Sta_Produto = row["Sta_Produto"].ToString() == "A" ? EStatus.A : EStatus.I;
                produtoCosifQueryResult.Cod_Cosif = row["Cod_Cosif"].ToString();
                produtoCosifQueryResult.Cod_Classificacao = row["Cod_Classificacao"].ToString();
                produtoCosifQueryResult.Sta_Cosif = row["Sta_Cosif"].ToString() == "A" ? EStatus.A : EStatus.I;

                produtosCosifsQueryResult.Add(produtoCosifQueryResult);
            }

            return produtosCosifsQueryResult;
        }

        public int ObterProximoLancamento(
            decimal datMes,
            decimal datAno,
            string codProduto,
            string codCosif)
        {
            _context.AdicionarParametro("@Dat_Mes", SqlDbType.Decimal, datMes);
            _context.AdicionarParametro("@Dat_Ano", SqlDbType.Decimal, datAno);
            _context.AdicionarParametro("@Cod_Produto", SqlDbType.Char, codProduto);
            _context.AdicionarParametro("@Cod_Cosif", SqlDbType.Char, codCosif);

            var dataTable = _context.ExecutarConsulta("spObterProximoLancamento");

            return (int)dataTable.Rows[0]["ProximoLancamento"];
        }
    }
}