namespace conferenceManagement.DTO
{
    public class AtendeeDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<int> Sessions { get; set; }
    }
}
