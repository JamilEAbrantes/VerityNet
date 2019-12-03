using FluentValidator.Validation;
using System.Collections.Generic;
using System.Linq;
using V8Net.Shared.Entities;
using VerityNet.Domain.MovimentoManualContext.Enums;

namespace VerityNet.Domain.MovimentoManualContext.Entities
{
    public class Produto : Entity
    {
        private readonly IList<ProdutoCosif> _produtosCosif;

        public Produto()
        {
            _produtosCosif = new List<ProdutoCosif>();
        }

        public Produto(
            string cod_Produto, 
            string des_Produto, 
            EStatus sta_Status)
        {
            Cod_Produto = cod_Produto;
            Des_Produto = des_Produto;
            Sta_Status = sta_Status;
            _produtosCosif = new List<ProdutoCosif>();

            AddNotifications(new ValidationContract()
                .Requires()
                .HasMaxLen(Cod_Produto, 4, "Cod_Produto", "O campo Cod_Produto deve conter no máximo 4 caracteres")
                .HasMinLen(Cod_Produto, 1, "Cod_Produto", "O campo Cod_Produto deve conter no mínimo 1 caracteres")
                .HasMaxLen(Des_Produto, 30, "Des_Produto", "O campo Des_Produto deve conter no máximo 30 caracteres")
                .HasMinLen(Des_Produto, 3, "Des_Produto", "O campo Des_Produto deve conter no mínimo 3 caracteres")
            );
        }

        public string Cod_Produto { get; private set; }
        public string Des_Produto { get; private set; }
        public EStatus Sta_Status { get; private set; }
        public IReadOnlyCollection<ProdutoCosif> ProdutosCosif => _produtosCosif.ToArray();

        public bool IndicadorProdutoInativado() =>
            Sta_Status == EStatus.I ? true : false;         

        public void AtribuirProdutosCosif(IEnumerable<ProdutoCosif> produtosCosif)
        {
            var produtos = produtosCosif.ToList();
            produtos.ForEach(x => _produtosCosif.Add(x));
        }

        public override string ToString() => 
            $"[ { GetType().Name } -> { Cod_Produto } { Des_Produto } - Status: { Sta_Status } ]";
    }
}