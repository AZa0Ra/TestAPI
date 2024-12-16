namespace api.Dtos
{
    public class IncidentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }
}