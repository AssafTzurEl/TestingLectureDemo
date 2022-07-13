using BankServer.Model;
using BankServer.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BankServer.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(ServiceExceptionFilterAttribute))]
    [Route("[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AccountController : ControllerBase
    {
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet("{accountId}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Account Get(int accountId)
        {
            return _accountService.Get(accountId);
        }

        [HttpGet("")]
        public IEnumerable<Account> GetAll()
        {
            return _accountService.GetAll();
        }

        [HttpPost("")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // when trying to add an account with ID != 0
        public ActionResult<Account> Add(Account account)
        {
            account = _accountService.Add(account);

            return Created($"{Request.Path}/{account.Id}", account);
        }

        [HttpPatch("{accountId}/credit/{amount}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Account Credit(int accountId, decimal amount)
        {
            var account = _accountService.Credit(accountId, amount);

            return account;
        }

        [HttpPatch("{accountId}/charge/{amount}")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Account Charge(int accountId, decimal amount)
        {
            var account = _accountService.Charge(accountId, amount);

            return account;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public void Delete(int accountId)
        {
            _accountService.Delete(accountId);
        }

        [HttpDelete("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public void DeleteAll()
        {
            _accountService.DeleteAll();
        }

        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;
    }
}