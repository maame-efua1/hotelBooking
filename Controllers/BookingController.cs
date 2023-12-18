using BookWithEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BookWithEase.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }   
           
        public IActionResult AvailableRooms(Room Room)
        {
                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                SqlConnection connection = new SqlConnection(connectionString);

                string query = ;

                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                List<Room> room = new List<Room>();


                while (reader.Read())
                {
                    Room rooms = new Room();
                    rooms.roomId = reader["roomId"].ToString();
                    rooms.room_no = reader["room_no"].ToString();
                    rooms.roomtype = reader["roomtype"].ToString();
                    rooms.pricePernight = reader["pricePernight"].ToString();
                    rooms.roomfloor = reader["roomfloor"].ToString();
                    rooms.roomSize = reader["roomSize"].ToString();


                    room.Add(rooms);
                }
                connection.Close();

                return View(room);
            }

            public IActionResult ConfirmBooking()
        {
            return View();
        }
    }
}
