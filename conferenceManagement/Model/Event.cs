namespace conferenceManagement.Model
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();
    }
}
