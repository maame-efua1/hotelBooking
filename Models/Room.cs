namespace BookWithEase.Models
{
    public class Room
    {
        public string roomId { get; set; }

        public string room_no  { get; set; }

        public string roomtype { get; set; }

        public string pricePernight { get; set; }

        public string roomSize { get; set; }

        public string roomfloor { get; set; }
        
        public int guests { get; set; }
        public string checkIn { get; set; }
        public string checkOut { get; set; }

        public DateTime dateBooked { get; set; } = DateTime.Now.Date;

    }
}
