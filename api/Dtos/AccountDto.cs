namespace api.Dtos
{
    public class AccountDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<ContactDto> Contacts { get; set; } = new List<ContactDto>();
    }
}