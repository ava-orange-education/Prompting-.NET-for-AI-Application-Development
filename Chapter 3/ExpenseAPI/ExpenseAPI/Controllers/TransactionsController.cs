using Microsoft.AspNetCore.Mvc;
using ExpenseAPI.Services;
using ExpenseAPI.Models;

namespace ExpenseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionLLM>>> Get()
    {
        try
        {
            var items = await _transactionService.GetAll();
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
