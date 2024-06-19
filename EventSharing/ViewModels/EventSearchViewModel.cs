namespace EventSharing.ViewModels
{
    public class EventSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public DateTime? StartDate { get; set; }
        public int? IdCategory { get; set; }
        public List<CategoryViewModel>? CategoriesVm { get; set; }
        public List<EventViewModel>? EventsVm { get; set; }
    }
}
