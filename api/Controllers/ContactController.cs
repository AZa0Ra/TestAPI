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
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDBContext _context;

        public ContactController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateContact(ContactDto dto)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == dto.AccountId);
            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Email == dto.Email);

            if (contact != null)
            {
                contact.FirstName = dto.FirstName;
                contact.LastName = dto.LastName;

                if (contact.AccountId != account.Id)
                {
                    contact.AccountId = account.Id;
                }
            }
            else
            {
                contact = new Contact
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    AccountId = account.Id
                };

                await _context.Contacts.AddAsync(contact);
            }

            await _context.SaveChangesAsync();

            return Ok(contact);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactById(int id)
        {
            var contact = await _context.Contacts
                .Include(c => c.Account)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contact == null)
            {
                return NotFound(new { message = "Contact not found." });
            }

            return Ok(new
            {
                contact.Id,
                contact.FirstName,
                contact.LastName,
                contact.Email,
                contact.AccountId
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContacts()
        {
            var contacts = await _context.Contacts
                .Include(c => c.Account)
                .ToListAsync();
            var result = contacts.Select(contact => contact.ToDto());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound(new { message = "Contact not found." });
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Contact deleted successfully." });
        }
    }
}