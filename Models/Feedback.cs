using Microsoft.AspNetCore.Http.HttpResults;

namespace BookWithEase.Models
{
    public class Feedback
    {
        public string f_name { get; set; }

        public string f_email { get; set; }

        public string f_number { get; set; }

        public string f_subject { get; set; }

        public string f_message { get; set; }

        public DateTime dateCreated { get; set; } = DateTime.Now.Date;

        public string FsuccessMessage { get; set; } = "Feedback sent successfully.";
    }
}
