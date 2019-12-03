using FluentValidator.Validation;
using System.Collections.Generic;
using System.Linq;
using V8Net.Shared.Entities;
using VerityNet.Domain.MovimentoManualContext.Enums;

namespace VerityNet.Domain.MovimentoManualContext.Entities
{
    public class ProdutoCosif : Entity
    {
        private readonly IList<MovimentoManual> _movimentosManuais;

        public ProdutoCosif()
        {
            _movimentosManuais = new List<MovimentoManual>();
        }

        public ProdutoCosif(
            Produto produto, 
            string cod_Cosif, 
            string cod_Classificacao, 
            EStatus sta_Status)
        {
            Produto = produto;
            Cod_Cosif = cod_Cosif;
            Cod_Classificacao = cod_Classificacao;
            Sta_Status = sta_Status;
            _movimentosManuais = new List<MovimentoManual>();

            AddNotifications(new ValidationContract()
                .Requires()
                .IsNotNull(Produto, "Produto", "Ao menos um produto deve ser informado")
                .HasMaxLen(Cod_Cosif, 11, "Cod_Cosif", "O campo Cod_Cosif deve conter no máximo 11 caracteres")
                .HasMinLen(Cod_Cosif, 1, "Cod_Cosif", "O campo Cod_Cosif deve conter no mínimo 1 caracter")
                .HasMaxLen(Cod_Classificacao, 6, "Cod_Classificacao", "O campo Cod_Classificacao deve conter no máximo 6 caracteres")
                .HasMinLen(Cod_Classificacao, 1, "Cod_Classificacao", "O campo Cod_Classificacao deve conter no mínimo 1 caracter")
            );
        }

        public Produto Produto { get; private set; }
        public string Cod_Cosif { get; private set; }
        public string Cod_Classificacao { get; private set; }
        public EStatus Sta_Status { get; private set; }
        public IEnumerable<MovimentoManual> MovimentosManuais;

        public bool IndicadorCosifInativado() =>
            Sta_Status == EStatus.I ? true : false;

        public void AtribuirMovimentosManuais(IEnumerable<MovimentoManual> movimentosManuais)
        {
            var movimentos = movimentosManuais.ToList();
            movimentos.ForEach(x => _movimentosManuais.Add(x));
        }

        public override string ToString() => 
            $"[ { GetType().Name } -> { Produto.Cod_Produto } { Cod_Cosif } - Status: { Sta_Status } ]";
    }
}