namespace ExpenseAPI.Models;

public class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}

public class TransactionLLM
{
    public string Category { get; set; } = string.Empty;
    public decimal percentage { get; set; }
}
