namespace conferenceManagement.Model
{
    public class Attendee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<SessionAttendee> SessionAttendees { get; set; } = new List<SessionAttendee>();
    }
}
