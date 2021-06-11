using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Get List of Accounts
        /// </summary>
        [Produces("application/json")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResult<List<Account>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result))]
        public IActionResult Get()
        {
            var result = _accountService.Get();

            if (!result.IsError)
            {
                result.HttpCode = Ok().StatusCode;
                return Ok(result);
            }

            result.HttpCode = BadRequest().StatusCode;
            return BadRequest(result);
        }

        /// <summary>
        /// Create Account
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     {     
        ///         "accountNumber": 123,
        ///         "currencyCode": "TRY",
        ///         "balance": 100
        ///     }
        /// 
        /// </remarks>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result))]
        public IActionResult Post(Account account)
        {            
            var result = _accountService.Create(account);

            if (!result.IsError)
            {
                result.HttpCode = StatusCodes.Status201Created;
                return CreatedAtAction(nameof(Post), result);
            }

            result.HttpCode = BadRequest().StatusCode;
            return BadRequest(result);
        }
    }
}