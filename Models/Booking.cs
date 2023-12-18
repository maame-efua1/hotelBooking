namespace BookWithEase.Models
{
    public class Booking
    {
        public DateOnly checkIn { get; set; }   

        public DateOnly CheckOut { get; set; }

        public decimal totalPrice { get; set; }

        public string customerId { get; set; }

        public string roomId { get; set; }

        public DateTime dateBooked { get; set; } = DateTime.Now;

        
    }
}
