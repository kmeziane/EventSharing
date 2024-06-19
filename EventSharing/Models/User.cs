using Microsoft.AspNetCore.Identity;

namespace EventSharing.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public List<Event>? JoinedEvents { get; set; }
        public List<Event>? CreatdEvents { get; set; }
    }
}
