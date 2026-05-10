using System.ComponentModel.DataAnnotations;

namespace HotelApp_Api.Models
{
    public class Reservation
    {
        [Key]
        public int IdReservation { get; set; }

        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        [MaxLength(20)]
        public string Statut { get; set; }

        public int IdClient { get; set; }
        public Client? Client { get; set; }

        public int IdChambre { get; set; }
        public Chambre? Chambre { get; set; }

        public Paiement? Paiement { get; set; }
    }
}