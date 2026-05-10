namespace HotelApp_Api.DTOs
{
    public class CreateReservationDto
    {
        public DateTime DateDebut { get; set; }

        public DateTime DateFin { get; set; }

        public int IdClient { get; set; }

        public int IdChambre { get; set; }
    }
}