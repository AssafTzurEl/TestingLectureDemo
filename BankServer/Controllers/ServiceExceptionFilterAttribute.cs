using BankServer.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BankServer.Controllers
{
    public class ServiceExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public ServiceExceptionFilterAttribute(ILogger<ServiceExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var statusCode = (int) (context.Exception switch
            {
                EntityNotFoundException ex => HttpStatusCode.NotFound,
                EntityAlreadyExistsException ex => HttpStatusCode.Conflict,
                ArgumentException ex => HttpStatusCode.BadRequest,
                Exception ex => LogServerError(ex)
            });

            context.Result = new ObjectResult(new { context.Exception.Message })
            {
                StatusCode = statusCode
            };
        }

        private HttpStatusCode LogServerError(Exception ex)
        {
            _logger.LogError(ex, ex.ToString());

            return HttpStatusCode.InternalServerError;
        }

        private readonly ILogger<ServiceExceptionFilterAttribute> _logger;
    }
}
