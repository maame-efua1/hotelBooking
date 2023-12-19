using BookWithEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookWithEase.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }   
           
        public IActionResult AvailableRooms( string guests, DateOnly checkIn, DateOnly checkOut)
        {
                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                SqlConnection connection = new SqlConnection(connectionString);

            string query = $"SELECT r.roomId, r.room_no, r.roomtype, r.pricePernight,r.roomfloor, r.roomSize FROM Room r LEFT JOIN Booking b ON r.roomId = b.roomId AND (b.CheckOut IS NULL OR b.CheckOut < DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}'))" +
                           $"WHERE r.roomId NOT IN(SELECT roomId FROM Booking WHERE (CheckIn < DATEADD(DAY, 1, '{checkOut.ToString("yyyy-MM-dd")}') AND CheckOut > DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}')) )";
                               //$"AND((r.roomtype = 'single' AND {guests} BETWEEN 1 AND 2) OR(r.roomtype = 'double' AND {guests} BETWEEN 3 AND 4)"+
                               //$"OR(r.roomtype = '' AND {guests} BETWEEN 5 AND 10) )";


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
