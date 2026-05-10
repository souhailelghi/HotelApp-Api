using System.ComponentModel.DataAnnotations;

namespace HotelApp_Api.Models
{
    public class Admin
    {
        [Key]
        public int IdAdmin { get; set; }

        [MaxLength(50)]
        public string Nom { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string MotDePasse { get; set; }

        public ICollection<Chambre>? Chambres { get; set; }
    }
}