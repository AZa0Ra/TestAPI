using Microsoft.AspNetCore.Mvc;
using api.Data;
using api.Dtos;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Mapper;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public AccountController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            var existingAccount = await _context.Accounts.FirstOrDefaultAsync(a => a.Name == dto.Name);
            if (existingAccount != null)
            {
                return Conflict(new { message = "Account with this name already exists." });
            }

            var account = new Account
            {
                Name = dto.Name
            };

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Contacts)
                .ToListAsync();

            var result = accounts.Select(account => account.ToDto());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Contacts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account.ToDto());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts
                .Include(a => a.Contacts)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            _context.Contacts.RemoveRange(account.Contacts);
            _context.Accounts.Remove(account);

            await _context.SaveChangesAsync();
            return Ok(new { message = "Account and related data deleted successfully." });
        }
    }
}