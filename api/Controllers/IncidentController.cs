using Microsoft.AspNetCore.Mvc;
using api.Models;
using api.Dtos;
using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Mapper;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IncidentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public IncidentController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateIncidentWithValidation(CreateIncidentRequestDto dto)
        {
            var account = await _context.Accounts
                .Include(a => a.Contacts)
                .FirstOrDefaultAsync(a => a.Name == dto.AccountName);

            if (account == null)
            {
                return NotFound(new { message = "Account not found." });
            }

            // Look by email
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Email == dto.ContactEmail);

            if (contact != null)
            {
                // Update
                contact.FirstName = dto.ContactFirstName;
                contact.LastName = dto.ContactLastName;

                // Subscibe to account
                if (contact.AccountId != account.Id)
                {
                    contact.AccountId = account.Id;
                    _context.Contacts.Update(contact);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                contact = new Contact
                {
                    FirstName = dto.ContactFirstName,
                    LastName = dto.ContactLastName,
                    Email = dto.ContactEmail,
                    AccountId = account.Id
                };
                await _context.Contacts.AddAsync(contact);
            }

            // New incident
            var incident = new Incident
            {
                Name = dto.IncidentDescription,
                Accounts = new List<Account> { account }
            };
            await _context.Incidents.AddAsync(incident);

            await _context.SaveChangesAsync();
            incident.Id = incident.Id;

            return CreatedAtAction(nameof(GetIncidentById), new { id = incident.Id }, incident.ToDto());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIncidentById(int id)
        {
            var incident = await _context.Incidents
                .Include(i => i.Accounts)
                .ThenInclude(a => a.Contacts)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (incident == null)
            {
                return NotFound();
            }

            return Ok(incident.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIncidents()
        {
            var incidents = await _context.Incidents
                .Include(i => i.Accounts)
                .ThenInclude(a => a.Contacts)
                .ToListAsync();

            var result = incidents.Select(incident => incident.ToDto());
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncident(int id)
        {
            var incident = await _context.Incidents.FindAsync(id);
            if (incident == null)
            {
                return NotFound(new { message = "Incident not found." });
            }

            _context.Incidents.Remove(incident);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Incident deleted successfully." });
        }
    }
}