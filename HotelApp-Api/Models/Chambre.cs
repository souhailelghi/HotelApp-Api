using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelApp_Api.Models
{
    public class Chambre
    {
        [Key]
        public int IdChambre { get; set; }

        // Basic Information
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        // Image
        public string ImageUrl { get; set; } = string.Empty;

        public int TotalImages { get; set; }

        // Services / Features
        public bool FreeBreakfast { get; set; }

        public bool FreeParking { get; set; }

        public bool FreeWifi { get; set; }

        public bool AirportTransferAvailable { get; set; }

        public bool LoyaltyProgramAvailable { get; set; }

        // Capacity
        public int Capacity { get; set; }

        // Beds
        public int SingleBeds { get; set; }

        public int KingBeds { get; set; }

        // Pricing
        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal OldPrice { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        // Booking Information
        public int Nights { get; set; }

        public bool TaxesIncluded { get; set; }

        public bool FreeCancellationAvailable { get; set; }

        // Additional Details
        [MaxLength(2000)]
        public string Details { get; set; } = string.Empty;

        // Status
        [MaxLength(20)]
        public string Statut { get; set; } = "Available";

        // Relations
        public int IdAdmin { get; set; }

        public Admin? Admin { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
    }
}