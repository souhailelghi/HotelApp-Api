namespace HotelApp_Api.DTOs
{
    public class CreateClientDto
    {
        public string Nom { get; set; }

        public string Prenom { get; set; }

        public string Email { get; set; }

        public string MotDePasse { get; set; }

        public string Telephone { get; set; }
    }
}