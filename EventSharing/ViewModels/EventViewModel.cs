
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace EventSharing.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères" )]
        public string? Name { get; set; }
        [Required(ErrorMessage = "La description est obligatoire")]
        [StringLength(500, ErrorMessage = "La description ne peut pas dépasser 500 caractères")]
        public string? Description { get; set; }
        [Required(ErrorMessage = "La date de début est obligatoire")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "La date de fin est obligatoire")]
        public DateTime? EndDate { get; set; }
        public int IdCategory { get; set; }
        public string? CategoryName { get; set; }     
        public List<CategoryViewModel>? CategoriesVm { get; set; }
        public bool IsCurrentUserRegistred { get; set; }
    }
}
