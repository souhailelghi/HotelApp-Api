using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp_Api.Models
{
    public class Chambre
    {
        [Key]
        public int IdChambre { get; set; }

        public int Numero { get; set; }

        [MaxLength(50)]
        public string Type { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Prix { get; set; }

        [MaxLength(20)]
        public string Statut { get; set; }

        public int IdAdmin { get; set; }
        public Admin? Admin { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}