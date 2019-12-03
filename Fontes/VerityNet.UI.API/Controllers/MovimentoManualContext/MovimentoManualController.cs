using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VerityNet.Domain.MovimentoManualContext.Commands.Handlers;
using VerityNet.Domain.MovimentoManualContext.Commands.Inputs;
using VerityNet.Domain.MovimentoManualContext.Commands.Outputs;
using VerityNet.Domain.MovimentoManualContext.Commands.Queries;
using VerityNet.Domain.MovimentoManualContext.Repositories;
using VerityNet.Shared.Commands;

namespace VerityNet.UI.API.Controllers.MovimentoManualContext
{
    public class MovimentoManualController : Controller
    {
        private readonly IMovimentoManualRepository _repository;
        private readonly MovimentoManualHandler _handler;

        public MovimentoManualController(
            IMovimentoManualRepository repository, 
            MovimentoManualHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/movimento-manual")]
        public MovimentoManualQueryResult MovimentoManual(
            decimal datMes,
            decimal datAno,
            decimal numLancemanto,
            string codProduto,
            string codCosif) =>
            _repository.MovimentoManual(datMes, datAno, numLancemanto, codProduto, codCosif);
        
        [HttpGet]
        [Route("v1/movimentos-manuais")]
        public IEnumerable<MovimentoManualQueryResult> ListarMovimentosManuais() => 
            _repository.ListarMovimentosManuais();

        [HttpPost]
        [Route("v1/novo-movimento-manual")]
        public ICommandResult NovoMovimetoManual([FromBody]CriarMovimentoManualCommand command) =>
            (CommandResult)_handler.Handler(command);

        [HttpGet]
        [Route("v1/produtos")]
        public IEnumerable<ProdutoQueryResult> ListarProdutos() =>
            _repository.ListarProdutos();

        [HttpGet]
        [Route("v1/produtos-cosifs")]
        public IEnumerable<ProdutoCosifQueryResult> ListarProdutosCosifs() =>
            _repository.ListarProdutosCosifs();
    }
}