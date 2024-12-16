using api.Dtos;
using api.Models;

namespace api.Mapper
{
    public static class Mapper
    {
        public static AccountDto ToDto(this Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                Name = account.Name,
                Contacts = account.Contacts.Select(c => c.ToDto()).ToList()
            };
        }

        public static ContactDto ToDto(this Contact contact)
        {
            return new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                AccountId = contact.AccountId
            };
        }

        public static IncidentDto ToDto(this Incident incident)
        {
            return new IncidentDto
            {
                Id = incident.Id,
                Name = incident.Name,
                Accounts = incident.Accounts.Select(a => new AccountDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Contacts = a.Contacts.Select(c => new ContactDto
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        AccountId = c.AccountId
                    }).ToList()
                }).ToList()
            };
        }
    }
}