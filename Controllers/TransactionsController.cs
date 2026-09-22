using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spendly.API.Data;
using Spendly.API.Models;

namespace Spendly.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly SpendlyDbContext _context;

    public TransactionsController(SpendlyDbContext context)
    {
        _context = context;
    }

    // GET: api/Transactions
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        var transactions = await _context.Transactions
            .OrderByDescending(x => x.TransactionDate)
            .ToListAsync();

        return Ok(transactions);
    }

    // GET: api/Transactions/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
        {
            return NotFound(new
            {
                message = "Transaction not found."
            });
        }

        return Ok(transaction);
    }

    // POST: api/Transactions
    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(
        Transaction transaction)
    {
        if (transaction.Amount <= 0)
        {
            return BadRequest(new
            {
                message = "Amount must be greater than 0."
            });
        }

        if (string.IsNullOrWhiteSpace(transaction.Description))
        {
            return BadRequest(new
            {
                message = "Description is required."
            });
        }

        if (string.IsNullOrWhiteSpace(transaction.Category))
        {
            return BadRequest(new
            {
                message = "Category is required."
            });
        }

        if (string.IsNullOrWhiteSpace(transaction.PaymentMethod))
        {
            return BadRequest(new
            {
                message = "Payment method is required."
            });
        }

        if (transaction.TransactionDate == default)
        {
            transaction.TransactionDate = DateTime.Now;
        }

        // This endpoint is currently for manual transactions.
        transaction.Source = "Manual";

        _context.Transactions.Add(transaction);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetTransaction),
            new { id = transaction.Id },
            transaction);
    }

    // PUT: api/Transactions/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(
        int id,
        Transaction updatedTransaction)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
        {
            return NotFound(new
            {
                message = "Transaction not found."
            });
        }

        if (updatedTransaction.Amount <= 0)
        {
            return BadRequest(new
            {
                message = "Amount must be greater than 0."
            });
        }

        transaction.Amount = updatedTransaction.Amount;
        transaction.Description = updatedTransaction.Description;
        transaction.Category = updatedTransaction.Category;
        transaction.TransactionDate = updatedTransaction.TransactionDate;
        transaction.PaymentMethod = updatedTransaction.PaymentMethod;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/Transactions/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(int id)
    {
        var transaction = await _context.Transactions
            .FirstOrDefaultAsync(x => x.Id == id);

        if (transaction == null)
        {
            return NotFound(new
            {
                message = "Transaction not found."
            });
        }

        _context.Transactions.Remove(transaction);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}