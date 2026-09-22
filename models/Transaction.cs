namespace Spendly.API.Models;

public class Transaction
{
    public int Id { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; } = "";

    public string Category { get; set; } = "";

    public DateTime TransactionDate { get; set; }

    public string PaymentMethod { get; set; } = "";

    public string Source { get; set; } = "Manual";
}