namespace HotelApp_Api.DTOs
{
    public class CreateChambreDto
    {
        // Basic Information
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Image
        public string ImageUrl { get; set; } = string.Empty;

        public int TotalImages { get; set; }

        // Services
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
        public decimal CurrentPrice { get; set; }

        public decimal OldPrice { get; set; }

        public decimal PricePerNight { get; set; }

        // Booking
        public int Nights { get; set; }

        public bool TaxesIncluded { get; set; }

        public bool FreeCancellationAvailable { get; set; }

        // Details
        public string Details { get; set; } = string.Empty;

        // Status
        public string Statut { get; set; } = "Available";

        public int IdAdmin { get; set; }
    }
}