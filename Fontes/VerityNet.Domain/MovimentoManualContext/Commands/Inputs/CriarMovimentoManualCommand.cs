using FluentValidator;
using FluentValidator.Validation;
using VerityNet.Shared.Commands;

namespace VerityNet.Domain.MovimentoManualContext.Commands.Inputs
{
    public class CriarMovimentoManualCommand : Notifiable, ICommand
    {
        public decimal Dat_Mes { get; set; }
        public decimal Dat_Ano { get; set; }
        public string Cod_Produto { get; set; }
        public string Cod_Cosif { get; set; }        
        public string Des_Descricao { get; set; }
        public decimal Val_Valor { get; set; }
        public string Cod_Usuario { get; set; }

        public bool IsValidCommand()
        {
            AddNotifications(new ValidationContract()
                .Requires()
                .IsGreaterThan(Dat_Mes, 0, "Dat_Mes", "O campo mês deve ser preenchido.")
                .IsGreaterThan(Dat_Ano, 0, "Dat_Ano", "O campo ano deve ser preenchido.")
                );

            return Valid;
        }
    }
}