using FluentValidator;
using VerityNet.Domain.MovimentoManualContext.Commands.Inputs;
using VerityNet.Domain.MovimentoManualContext.Commands.Outputs;
using VerityNet.Domain.MovimentoManualContext.Entities;
using VerityNet.Domain.MovimentoManualContext.Enums;
using VerityNet.Domain.MovimentoManualContext.Repositories;
using VerityNet.Domain.MovimentoManualContext.ValueObjects;
using VerityNet.Shared.Commands;

namespace VerityNet.Domain.MovimentoManualContext.Commands.Handlers
{
    public class MovimentoManualHandler :
        Notifiable,
        ICommandHandler<CriarMovimentoManualCommand>
    {
        private readonly IMovimentoManualRepository _repository;

        public MovimentoManualHandler(IMovimentoManualRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handler(CriarMovimentoManualCommand command)
        {
            if (!command.IsValidCommand())
                return new CommandResult(false, "Por favor, verificar os campos abaixo", command.Notifications);

            // Verifica se o Produto é válido
            var produto = _repository.Produto(command.Cod_Produto, EStatus.A);
            if (produto == null)
                return new CommandResult(false, $"Produto não existe na base de dados. Cód. informado: { command.Cod_Produto }", null);
            else if (produto.IndicadorProdutoInativado())
                AddNotification("Produto", $"Produto inativado no momento. Prod. informado: { produto.Cod_Produto } - { produto.Des_Produto }");

            // Verifica se o cosif é válido
            var cosif = _repository.ProdutoCosif(command.Cod_Cosif, EStatus.A);
            if (cosif == null)
                return new CommandResult(false, $"Cosif não existe na base de dados. Cód. informado: { command.Cod_Produto }", null);
            else if (cosif.IndicadorCosifInativado())
                AddNotification("Cosif", $"Cosif inativado no momento. Cosif informado: { cosif.Cod_Cosif } -> Classificação: { cosif.Cod_Classificacao }");
                        
            var usuario = new UsuarioVO(command.Cod_Usuario);

            // Obtém o número de lancamento
            var numeroLancamento = _repository.ObterProximoLancamento(
                command.Dat_Mes,
                command.Dat_Ano,
                produto.Cod_Produto,
                cosif.Cod_Cosif);

            // Mapeia o Movimento Manual
            var movimentoManual = new MovimentoManual(
                command.Dat_Mes,
                command.Dat_Ano,
                numeroLancamento,
                command.Des_Descricao,
                produto,
                cosif,
                usuario,
                command.Val_Valor);

            AddNotifications(usuario, movimentoManual);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrigir os campos abaixo", Notifications);

            _repository.Salvar(movimentoManual);

            return new CommandResult(true, "Movimento manual cadastrado com sucesso", new { movimentoManual });
        }
    }
}