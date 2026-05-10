using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp_Api.Models
{
    public class Client
    {
        [Key]
        public int IdClient { get; set; }

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

        public ICollection<Reservation>? Reservations { get; set; }
    }
}