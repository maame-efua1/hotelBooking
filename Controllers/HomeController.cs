using BookWithEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace BookWithEase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Feedback User)
        {

            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO Feedback (f_name, f_email, f_number, f_subject, f_message, dateCreated) VALUES (@f_name, @f_email, @f_number, @f_subject, @f_message, @datecreated)";

                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@f_name", User.f_name);
                command.Parameters.AddWithValue("@f_email", User.f_email);
                command.Parameters.AddWithValue("@f_number", User.f_number);
                command.Parameters.AddWithValue("@f_subject", User.f_subject);
                command.Parameters.AddWithValue("@f_message", User.f_message);
                command.Parameters.AddWithValue("@dateCreated", User.dateCreated);

                
                    
                    command.ExecuteNonQuery();


                    connection.Close();
                
            }

            TempData["FsuccessMessage"] = "Feedback sent successfully.";
            return View();
        }
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
