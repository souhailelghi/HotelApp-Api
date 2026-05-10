using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp_Api.Models
{
    public class Paiement
    {
        [Key]
        public int IdPaiement { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Montant { get; set; }

        public DateTime DatePaiement { get; set; }

        [MaxLength(50)]
        public string Methode { get; set; }

        public int IdReservation { get; set; }
        public Reservation? Reservation { get; set; }
    }
}