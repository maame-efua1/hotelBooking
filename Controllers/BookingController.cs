using BookWithEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookWithEase.Controllers
{
    public class BookingController : Controller
    {
        private const string checkin = "";
        private const string checkout = "";
        private const string datebooked = "";
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AvailableRooms(string guests, DateOnly checkIn, DateOnly checkOut)
        {

            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            SqlConnection connection = new SqlConnection(connectionString);

            string query = $"SELECT r.roomId, r.room_no, r.roomtype, r.pricePernight,r.roomfloor, r.roomSize FROM Room r LEFT JOIN Booking b ON r.roomId = b.roomId AND (b.CheckOut IS NULL OR b.CheckOut < DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}'))" +
                           $"WHERE r.roomId NOT IN(SELECT roomId FROM Booking WHERE (CheckIn < DATEADD(DAY, 1, '{checkOut.ToString("yyyy-MM-dd")}') AND CheckOut > DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}')) )"+
            $"AND((r.roomSize = 'small' AND {guests} BETWEEN 1 AND 2) OR(r.roomSize = 'medium' AND {guests} BETWEEN 3 AND 4)" +
            $"OR(r.roomSize = 'large' AND {guests} BETWEEN 5 AND 10) )";

            HttpContext.Session.SetString("checkin", checkIn.ToString());
            HttpContext.Session.SetString("checkout", checkOut.ToString());

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

        [HttpPost]
        public IActionResult ConfirmBooking(int roomId, Booking Booking, Customer User)
        {
            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Booking (CheckIn, checkOut, totalPrice, roomId, /*customerId,*/ dateBooked)" +
                               "VALUES (@checkIn, @checkOut, @totalPrice, @roomId, /*@customerId,*/ @dateBooked);" +
                               "INSERT INTO Customer (firstname, lastname, email, phone, dateCreated)" +
                               "VALUES (@firstname, @lastname, @email, @phone, @dateCreated)";


                SqlCommand command = new SqlCommand(query, connection);


                command.Parameters.AddWithValue("@firstname", User.firstname);
                command.Parameters.AddWithValue("@lastname", User.lastname);
                command.Parameters.AddWithValue("@email", User.email);
                command.Parameters.AddWithValue("@phone", User.phone);
                command.Parameters.AddWithValue("@phone", User.dateCreated);
                command.Parameters.AddWithValue("@checkIn", Booking.checkIn);
                command.Parameters.AddWithValue("@checkOut", Booking.checkOut);
                command.Parameters.AddWithValue("@dateBooked", Booking.dateBooked);
                command.Parameters.AddWithValue("@totalPrice", Booking.totalPrice);
                command.Parameters.AddWithValue("@roomId", roomId);

                connection.Open();
                command.ExecuteNonQuery();

                return RedirectToAction("Index", "Home");

                connection.Close();

            }
            return View();
        }
        public IActionResult ConfirmBooking(int roomid)
        {
            var checkIn = HttpContext.Session.GetString("checkin");
            var checkOut = HttpContext.Session.GetString("checkout");

            var booking = new Booking();
            booking.checkIn = checkIn;
            booking.checkOut = checkOut;
           

            string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                try
                {
                    string query = $"Select * from Room where roomId={roomid}";

                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();


                    reader.Read();
                    var roomId = reader["roomId"].ToString();
                    var room_no = reader["room_no"].ToString();
                    var roomtype = reader["roomtype"].ToString();
                    var pricePernight = reader["pricePernight"].ToString();
                    var roomfloor = reader["roomfloor"].ToString();
                    var roomSize = reader["roomSize"].ToString();


                    var room = new Room()
                    {
                        roomId = roomId,
                        room_no = room_no,
                        roomtype = roomtype,
                        pricePernight = pricePernight,
                        roomfloor = roomfloor,
                        roomSize = roomSize,
                        checkIn = checkIn,
                        checkOut = checkOut,
                        dateBooked = DateTime.Now.Date

                    };

                    ViewData["checkin"] = checkIn;

                    return View(room);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);

                }
                finally
                {
                    connection.Close();
                }
            }

            return View(booking);
        }
    }
}
