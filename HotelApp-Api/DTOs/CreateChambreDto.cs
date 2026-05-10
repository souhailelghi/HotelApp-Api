namespace HotelApp_Api.DTOs
{
    public class CreateChambreDto
    {
        public int Numero { get; set; }

        public string Type { get; set; }

        public decimal Prix { get; set; }

        public string Statut { get; set; }

        public int IdAdmin { get; set; }
    }
}