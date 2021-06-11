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
    public class TransactionsController : ControllerBase
    {
        readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get List of Money Transactions
        /// </summary>
        [Produces("application/json")]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResult<List<Transaction>>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result))]
        public IActionResult Get()
        {
            var result = _transactionService.Get();

            if (!result.IsError)
            {
                result.HttpCode = Ok().StatusCode;
                return Ok(result);
            }

            result.HttpCode = BadRequest().StatusCode;
            return BadRequest(result);
        }

        /// <summary>
        /// Create Money Transaction
        /// </summary>
        /// <remarks>
        /// Sample Request:
        /// 
        ///     {     
        ///         "senderAccountNumber": 123,
        ///         "receiverAccountNumber": 156,
        ///         "amount": 23.45
        ///     }
        /// 
        /// </remarks>
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Result))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Result))]
        public IActionResult Post(Transaction transaction)
        {
            var result = _transactionService.Create(transaction);

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