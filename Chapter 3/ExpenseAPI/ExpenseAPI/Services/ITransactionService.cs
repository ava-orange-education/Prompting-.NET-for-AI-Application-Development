using ExpenseAPI.Models;

namespace ExpenseAPI.Services;

public interface ITransactionService
{
    Task<List<TransactionLLM>> GetAll();
}
