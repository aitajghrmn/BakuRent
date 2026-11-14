using System.ComponentModel.DataAnnotations;

namespace BakuRentWeb.Models
{
    public class Listing
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Başlıq daxil edilməlidir")]
        [StringLength(120, ErrorMessage = "Başlıq maksimum 120 simvol ola bilər")]
        public string Title { get; set; } = "";

        [Required(ErrorMessage = "Təsvir daxil edilməlidir")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Rayon seçilməlidir")]
        public string District { get; set; } = "";

        [Range(1, 10, ErrorMessage = "Otaq sayı 1 ilə 10 arasında olmalıdır")]
        public int Rooms { get; set; } = 1;

        [Range(50, 20000, ErrorMessage = "Qiymət 50 ilə 20000 AZN arasında olmalıdır")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Telefon nömrəsi daxil edilməlidir")]
        [RegularExpression(@"^\+?[0-9\s\-]{9,15}$",
            ErrorMessage = "Telefon nömrəsi düzgün formatda deyil")]
        public string Phone { get; set; } = "";

        // "Active" or "Rented"
        public string Status { get; set; } = "Active";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
