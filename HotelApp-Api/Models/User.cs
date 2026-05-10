using System.ComponentModel.DataAnnotations;

namespace HotelApp_Api.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [MaxLength(50)]
        public string Nom { get; set; }

        [MaxLength(50)]
        public string Prenom { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string MotDePasse { get; set; }

        [MaxLength(20)]
        public string Telephone { get; set; }
    }
}
