using FluentValidator.Validation;
using System;
using V8Net.Shared.Entities;
using VerityNet.Domain.MovimentoManualContext.ValueObjects;

namespace VerityNet.Domain.MovimentoManualContext.Entities
{
    public class MovimentoManual : Entity
    {
        public MovimentoManual() { }

        public MovimentoManual(
            decimal dat_Mes, 
            decimal dat_Ano, 
            decimal num_Lancemanto, 
            string des_Descricao, 
            Produto produto, 
            ProdutoCosif produtoCosif,
            UsuarioVO usuarioVO, 
            decimal val_Valor)
        {
            Dat_Mes = dat_Mes;
            Dat_Ano = dat_Ano;
            Num_Lancemanto = num_Lancemanto;
            Des_Descricao = des_Descricao;
            Produto = produto;
            ProdutoCosif = produtoCosif;
            Dat_Movimento = DateTime.Now;
            UsuarioVO = usuarioVO;
            Val_Valor = val_Valor;

            AddNotifications(new ValidationContract()
                .Requires()
                .IsNotNull(Produto, "Produto", "Ao menos um produto deve ser informado")
                .IsNotNull(ProdutoCosif, "ProdutoCosif", "Ao menos um cosif deve ser informado")
                .IsNotNull(UsuarioVO, "UsuarioVO", "Ao menos um usuário deve ser informado")
                .HasMaxLen(Des_Descricao, 50, "Des_Descricao", "O campo Des_Descricao deve conter no máximo 50 caracteres")
                .HasMinLen(Des_Descricao, 3, "Des_Descricao", "O campo Des_Descricao deve conter no mínimo 3 caracteres")
                .IsGreaterThan(Val_Valor, 0, "Val_Valor", "O campo Val_Valor deve ser preenchido")
            );
        }

        public decimal Dat_Mes { get; private set; }
        public decimal Dat_Ano { get; private set; }
        public decimal Num_Lancemanto { get; private set; }
        public string Des_Descricao { get; private set; }
        public Produto Produto { get; private set; }
        public ProdutoCosif ProdutoCosif { get; private set; }
        public DateTime Dat_Movimento { get; private set; }
        public UsuarioVO UsuarioVO { get; private set; }
        public decimal Val_Valor { get; private set; }
        
        public override string ToString() => 
            $"[ { GetType().Name } -> Prod.: { Produto.Cod_Produto } { Produto.Des_Produto } - Cosif: { ProdutoCosif.Cod_Cosif } ]";
    }
}