using BookWithEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Transactions;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace BookWithEase.Controllers
{
    public class BookingController : Controller
    {
        private const string checkin = "";
        private const string checkout = "";
        private const string datebooked = "";

        public IActionResult Index( )
        {
            return View();
        }

        public IActionResult AvailableRooms(string guests, DateOnly checkIn, DateOnly checkOut)
            {

                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                SqlConnection connection = new SqlConnection(connectionString);

                string query = $"SELECT r.roomId, r.room_no, r.roomtype, r.pricePernight,r.roomfloor, r.roomSize FROM Room r LEFT JOIN Booking b ON r.roomId = b.roomId AND (b.CheckOut IS NULL OR b.CheckOut < DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}'))" +
                               $"WHERE r.roomId NOT IN(SELECT roomId FROM Booking WHERE (CheckIn < DATEADD(DAY, 1, '{checkOut.ToString("yyyy-MM-dd")}') AND CheckOut > DATEADD(DAY, -1, '{checkIn.ToString("yyyy-MM-dd")}')) )" +
                $"AND((r.roomSize = 'small' AND {guests} BETWEEN 1 AND 2) OR(r.roomSize = 'medium' AND {guests} BETWEEN 3 AND 5)" +
                $"OR(r.roomSize = 'large' AND {guests} BETWEEN 6 AND 10) )";

                HttpContext.Session.SetString("checkin", checkIn.ToString());
                HttpContext.Session.SetString("checkout", checkOut.ToString());
                HttpContext.Session.SetString("datebooked", DateTime.Now.Date.ToString("dd/MM/yyyy"));

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

            public IActionResult ConfirmBooking(int roomid)
            {
                var checkIn = HttpContext.Session.GetString("checkin");
                var checkOut = HttpContext.Session.GetString("checkout");
                var dateBooked = HttpContext.Session.GetString("datebooked");

                var booking = new Booking();
                booking.checkIn = checkIn;
                booking.checkOut = checkOut;
                booking.dateBooked = dateBooked;


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
                            dateBooked = dateBooked

                        };

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

            [HttpPost]
            public IActionResult ConfirmBooking(int roomid, Booking Booking, Customer User)
            {
                string connectionString = "Server=LAPTOP-LIL017KH\\SQLEXPRESS;Database=BookWithEase;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string queryCustomer = "INSERT INTO Customer (firstName, lastName, Email, Phone, DateCreated) " +
                                      "VALUES (@firstname, @lastname, @email, @phone, @dateCreated);";


                    SqlCommand command = new SqlCommand(queryCustomer, connection);


                    command.Parameters.AddWithValue("@firstname", User.firstname);
                    command.Parameters.AddWithValue("@lastname", User.lastname);
                    command.Parameters.AddWithValue("@email", User.email);
                    command.Parameters.AddWithValue("@phone", User.phone);
                    command.Parameters.AddWithValue("@dateCreated", User.dateCreated);



                    string queryBooking = "INSERT INTO Booking (CheckIn, CheckOut, totalPrice, customerId, roomId, dateBooked) " +
                                                   "VALUES (@checkIn, @checkOut, @totalPrice, @customerId, @roomId, @dateBooked);";

                    SqlCommand command2 = new SqlCommand(queryBooking, connection);

                    command.Parameters.AddWithValue("@checkIn", Booking.checkIn);
                    command.Parameters.AddWithValue("@checkOut", Booking.checkOut);
                    command.Parameters.AddWithValue("@dateBooked", Booking.dateBooked);
                    command.Parameters.AddWithValue("@totalPrice", Booking.totalPrice);
                    command.Parameters.AddWithValue("@roomId", roomid);


                    command.ExecuteNonQuery();

                    connection.Close();

                }
                TempData["successMessage"] = "Room has been successfully booked. You'll recieve an email with your booking details. Thank you.";
                return RedirectToAction("Index", "Home");
            }
            
        }
    }

