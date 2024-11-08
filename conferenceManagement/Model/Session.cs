namespace conferenceManagement.Model
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
        public ICollection<SessionAttendee> SessionAttendees { get; set; } = new List<SessionAttendee>();
    }
}
