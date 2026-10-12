using System.ComponentModel.DataAnnotations;

namespace AjaxSecurityApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название обязательно")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Цена от 0.01 до 1000000")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Категория")]
        public string Category { get; set; }
    }
}
