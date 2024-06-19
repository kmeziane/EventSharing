namespace EventSharing.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Category? Category { get; set; }
        public User? Creator { get; set; }
        public List<User>? Participants { get; set; }
    }
}
