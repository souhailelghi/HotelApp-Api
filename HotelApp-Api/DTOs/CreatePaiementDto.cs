namespace HotelApp_Api.DTOs
{
    public class CreatePaiementDto
    {
        public decimal Montant { get; set; }

        public string Methode { get; set; }

        public int IdReservation { get; set; }
    }
}