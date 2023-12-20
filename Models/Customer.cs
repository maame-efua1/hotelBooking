namespace BookWithEase.Models
{
    public class Customer
    {
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }

        public string phone { get; set; }

        public DateTime dateCreated { get; set; } = DateTime.Now.Date;
    }
}
