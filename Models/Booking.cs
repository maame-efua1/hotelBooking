namespace BookWithEase.Models
{
    public class Booking
    {
        public string checkIn { get; set; }   

        public string checkOut { get; set; }

        public decimal totalPrice { get; set; }

        public string customerId { get; set; }

        public string stayDuration { get; set; }

        public string dateBooked { get; set; } = DateTime.Now.Date.ToString("dd/MM/yyyy");
        public DateTime dateBooked1 { get; set; } = DateTime.Now.Date;


    }
}
