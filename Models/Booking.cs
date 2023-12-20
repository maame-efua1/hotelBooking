namespace BookWithEase.Models
{
    public class Booking
    {
        public string checkIn { get; set; }   

        public string checkOut { get; set; }

        public decimal totalPrice { get; set; }

        public string customerId { get; set; }

        public string roomId { get; set; }

        public DateTime dateBooked { get; set; } = DateTime.Now.Date;

        
    }
}
