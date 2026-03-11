using API_ELEC_2.Models;
using Microsoft.Data.SqlClient;

namespace API_ELEC_2.Repositories
{
    public class BookingRepository
    {
        private readonly string _connectionString;

        public BookingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // ==================== BOOKING_DETAILS ====================

        public IEnumerable<Booking_Details> GetAllBookings()
        {
            var bookings = new List<Booking_Details>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT BookingID, FlightID FROM Booking_Details";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        bookings.Add(new Booking_Details
                        {
                            BookingID = (int)reader["BookingID"],
                            FlightID = (int)reader["FlightID"]
                        });
                    }
                }
            }
            return bookings;
        }

        public Booking_Details? GetBookingByID(int id)
        {
            Booking_Details? booking = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT BookingID, FlightID FROM Booking_Details WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            booking = new Booking_Details
                            {
                                BookingID = (int)reader["BookingID"],
                                FlightID = (int)reader["FlightID"]
                            };
                        }
                    }
                }
            }
            return booking;
        }

        public bool CreateBooking(Booking_Details booking)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Booking_Details (FlightID) VALUES (@FlightID)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FlightID", booking.FlightID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBooking(int id, Booking_Details booking)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Booking_Details SET FlightID = @FlightID WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", id);
                    cmd.Parameters.AddWithValue("@FlightID", booking.FlightID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBooking(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Booking_Details WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==================== BOOKING_PAY ====================

        public IEnumerable<Booking_Pay> GetAllBookingPay()
        {
            var list = new List<Booking_Pay>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PayID, BookingID, LastName, FirstName, MiddleName, Contact, Birthdate, Age FROM Booking_Pay";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Booking_Pay
                        {
                            PayID = (int)reader["PayID"],
                            BookingID = (int)reader["BookingID"],
                            LastName = reader["LastName"]?.ToString() ?? string.Empty,
                            FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                            MiddleName = reader["MiddleName"]?.ToString() ?? string.Empty,
                            Contact = reader["Contact"]?.ToString() ?? string.Empty,
                            Birthdate = reader["Birthdate"] == DBNull.Value ? DateOnly.MinValue : DateOnly.FromDateTime(Convert.ToDateTime(reader["Birthdate"])),
                            Age = reader["Age"] == DBNull.Value ? 0 : (int)reader["Age"]
                        });
                    }
                }
            }
            return list;
        }

        public Booking_Pay? GetBookingPayByID(int id)
        {
            Booking_Pay? pay = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PayID, BookingID, LastName, FirstName, MiddleName, Contact, Birthdate, Age FROM Booking_Pay WHERE PayID = @PayID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PayID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pay = new Booking_Pay
                            {
                                PayID = (int)reader["PayID"],
                                BookingID = (int)reader["BookingID"],
                                LastName = reader["LastName"]?.ToString() ?? string.Empty,
                                FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                                MiddleName = reader["MiddleName"]?.ToString() ?? string.Empty,
                                Contact = reader["Contact"]?.ToString() ?? string.Empty,
                                Birthdate = reader["Birthdate"] == DBNull.Value ? DateOnly.MinValue : DateOnly.FromDateTime(Convert.ToDateTime(reader["Birthdate"])),
                                Age = reader["Age"] == DBNull.Value ? 0 : (int)reader["Age"]
                            };
                        }
                    }
                }
            }
            return pay;
        }

        public IEnumerable<Booking_Pay> GetBookingPayByBookingID(int bookingId)
        {
            var list = new List<Booking_Pay>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT PayID, BookingID, LastName, FirstName, MiddleName, Contact, Birthdate, Age FROM Booking_Pay WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Booking_Pay
                            {
                                PayID = (int)reader["PayID"],
                                BookingID = (int)reader["BookingID"],
                                LastName = reader["LastName"]?.ToString() ?? string.Empty,
                                FirstName = reader["FirstName"]?.ToString() ?? string.Empty,
                                MiddleName = reader["MiddleName"]?.ToString() ?? string.Empty,
                                Contact = reader["Contact"]?.ToString() ?? string.Empty,
                                Birthdate = reader["Birthdate"] == DBNull.Value ? DateOnly.MinValue : DateOnly.FromDateTime(Convert.ToDateTime(reader["Birthdate"])),
                                Age = reader["Age"] == DBNull.Value ? 0 : (int)reader["Age"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool CreateBookingPay(Booking_Pay pay)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"INSERT INTO Booking_Pay (BookingID, LastName, FirstName, MiddleName, Contact, Birthdate, Age)
                                 VALUES (@BookingID, @LastName, @FirstName, @MiddleName, @Contact, @Birthdate, @Age)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", pay.BookingID);
                    cmd.Parameters.AddWithValue("@LastName", pay.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", pay.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", pay.MiddleName);
                    cmd.Parameters.AddWithValue("@Contact", pay.Contact);
                    cmd.Parameters.AddWithValue("@Birthdate", pay.Birthdate.ToDateTime(TimeOnly.MinValue));
                    cmd.Parameters.AddWithValue("@Age", pay.Age);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBookingPay(int id, Booking_Pay pay)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"UPDATE Booking_Pay SET BookingID = @BookingID, LastName = @LastName, FirstName = @FirstName,
                                 MiddleName = @MiddleName, Contact = @Contact, Birthdate = @Birthdate, Age = @Age
                                 WHERE PayID = @PayID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PayID", id);
                    cmd.Parameters.AddWithValue("@BookingID", pay.BookingID);
                    cmd.Parameters.AddWithValue("@LastName", pay.LastName);
                    cmd.Parameters.AddWithValue("@FirstName", pay.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", pay.MiddleName);
                    cmd.Parameters.AddWithValue("@Contact", pay.Contact);
                    cmd.Parameters.AddWithValue("@Birthdate", pay.Birthdate.ToDateTime(TimeOnly.MinValue));
                    cmd.Parameters.AddWithValue("@Age", pay.Age);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBookingPay(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Booking_Pay WHERE PayID = @PayID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PayID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==================== BOOKING_OTHERS ====================

        public IEnumerable<Booking_Others> GetAllBookingOthers()
        {
            var list = new List<Booking_Others>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Type FROM Booking_Others";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Booking_Others
                        {
                            ID = (int)reader["ID"],
                            Type = reader["Type"]?.ToString() ?? string.Empty
                        });
                    }
                }
            }
            return list;
        }

        public Booking_Others? GetBookingOthersByID(int id)
        {
            Booking_Others? item = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ID, Type FROM Booking_Others WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new Booking_Others
                            {
                                ID = (int)reader["ID"],
                                Type = reader["Type"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return item;
        }

        public bool CreateBookingOthers(Booking_Others others)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Booking_Others (Type) VALUES (@Type)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Type", others.Type);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBookingOthers(int id, Booking_Others others)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Booking_Others SET Type = @Type WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@Type", others.Type);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBookingOthers(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Booking_Others WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // ==================== BOOKING_OTHER_DETAILS ====================

        public IEnumerable<Booking_OtherDetails> GetAllBookingOtherDetails()
        {
            var list = new List<Booking_OtherDetails>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ID, BookingID, TypeID FROM Booking_Other_Details";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Booking_OtherDetails
                        {
                            ID = (int)reader["ID"],
                            BookingID = (int)reader["BookingID"],
                            TypeID = (int)reader["TypeID"]
                        });
                    }
                }
            }
            return list;
        }

        public Booking_OtherDetails? GetBookingOtherDetailsByID(int id)
        {
            Booking_OtherDetails? item = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ID, BookingID, TypeID FROM Booking_Other_Details WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            item = new Booking_OtherDetails
                            {
                                ID = (int)reader["ID"],
                                BookingID = (int)reader["BookingID"],
                                TypeID = (int)reader["TypeID"]
                            };
                        }
                    }
                }
            }
            return item;
        }

        public IEnumerable<Booking_OtherDetails> GetBookingOtherDetailsByBookingID(int bookingId)
        {
            var list = new List<Booking_OtherDetails>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT ID, BookingID, TypeID FROM Booking_Other_Details WHERE BookingID = @BookingID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Booking_OtherDetails
                            {
                                ID = (int)reader["ID"],
                                BookingID = (int)reader["BookingID"],
                                TypeID = (int)reader["TypeID"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public bool CreateBookingOtherDetails(Booking_OtherDetails details)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Booking_Other_Details (BookingID, TypeID) VALUES (@BookingID, @TypeID)";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@BookingID", details.BookingID);
                    cmd.Parameters.AddWithValue("@TypeID", details.TypeID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateBookingOtherDetails(int id, Booking_OtherDetails details)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "UPDATE Booking_Other_Details SET BookingID = @BookingID, TypeID = @TypeID WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.Parameters.AddWithValue("@BookingID", details.BookingID);
                    cmd.Parameters.AddWithValue("@TypeID", details.TypeID);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteBookingOtherDetails(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Booking_Other_Details WHERE ID = @ID";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}