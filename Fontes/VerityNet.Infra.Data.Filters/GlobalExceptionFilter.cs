using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;
using VerityNet.Domain.MovimentoManualContext.Commands.Outputs;

namespace VerityNet.Infra.Data.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public GlobalExceptionFilter(
            ILogger<GlobalExceptionFilter> logger, 
            IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            response.ContentType = "application/json";
            context.ExceptionHandled = true;
            context.Result = new JsonResult(new CommandResult(false, $"Ops... Houve um erro -> { context.Exception.Message }", null));
        }
    }
}