using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class BookingRepository
    {
        private readonly string _connectionString;

        public BookingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public Booking GetBookingByID(int id)
        {
            Booking booking = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT BookingID, FlightID, BookingDate FROM Bookings WHERE BookingID = @BookingID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            booking = new Booking
                            {
                                BookingID   = (int)reader["BookingID"],
                                FlightID    = (int)reader["FlightID"],
                                BookingDate = Convert.ToDateTime(reader["BookingDate"])
                            };
                        }
                    }
                }
            }
            return booking;
        }

        public IEnumerable<AvailableFlight> GetAvailableFlights()
        {
            var list = new List<AvailableFlight>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT f.FlightID,
                           orig.Code        AS OriginCode,
                           orig.Description AS OriginName,
                           dest.Code        AS DestinationCode,
                           dest.Description AS DestinationName,
                           f.TravelDate, f.TravelTime,
                           a.AirlineCode,
                           (f.MaxPax - f.CurrentPax) AS SeatsLeft
                    FROM FD_Details f
                    JOIN FD_Airline a         ON f.AirlineID     = a.AirlineID
                    JOIN FD_OtherDetails orig ON f.OriginID      = orig.OtherDetailsID
                    JOIN FD_OtherDetails dest ON f.DestinationID = dest.OtherDetailsID
                    WHERE f.TravelDate >= CAST(GETDATE() AS DATE)
                      AND (f.MaxPax - f.CurrentPax) > 0";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new AvailableFlight
                        {
                            FlightID        = (int)reader["FlightID"],
                            OriginCode      = reader["OriginCode"]?.ToString()      ?? string.Empty,
                            OriginName      = reader["OriginName"]?.ToString()      ?? string.Empty,
                            DestinationCode = reader["DestinationCode"]?.ToString() ?? string.Empty,
                            DestinationName = reader["DestinationName"]?.ToString() ?? string.Empty,
                            TravelDate      = Convert.ToDateTime(reader["TravelDate"]),
                            TravelTime      = reader["TravelTime"]?.ToString()      ?? string.Empty,
                            AirlineCode     = reader["AirlineCode"]?.ToString()     ?? string.Empty,
                            SeatsLeft       = (int)reader["SeatsLeft"]
                        });
                    }
                }
            }
            return list;
        }

        public IEnumerable<BookingDetail> GetBookingDetails(int bookingId)
        {
            var list = new List<BookingDetail>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT b.BookingID,
                           p.PaxID,
                           (p.FirstName + ' ' + p.LastName) AS FullName,
                           p.Contact,
                           b.FlightID,
                           f.TravelDate,
                           f.TravelTime
                    FROM Bookings b
                    JOIN Booking_Pax p ON b.BookingID = p.BookingID
                    JOIN FD_Details f  ON b.FlightID  = f.FlightID
                    WHERE b.BookingID = @BookingID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new BookingDetail
                            {
                                BookingID  = (int)reader["BookingID"],
                                PaxID      = (int)reader["PaxID"],
                                FullName   = reader["FullName"]?.ToString()  ?? string.Empty,
                                Contact    = reader["Contact"]?.ToString()   ?? string.Empty,
                                FlightID   = (int)reader["FlightID"],
                                TravelDate = Convert.ToDateTime(reader["TravelDate"]),
                                TravelTime = reader["TravelTime"]?.ToString() ?? string.Empty
                            });
                        }
                    }
                }
            }
            return list;
        }

        // Returns new BookingID
        public int CreateBooking(CreateBookingRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Insert Booking
                        string insertBooking = @"INSERT INTO Bookings (FlightID, BookingDate)
                                                 VALUES (@FlightID, GETDATE());
                                                 SELECT SCOPE_IDENTITY();";
                        int newBookingID;
                        using (var command = new SqlCommand(insertBooking, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FlightID", request.FlightID);
                            newBookingID = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // 2. Insert Pax
                        string insertPax = @"INSERT INTO Booking_Pax
                                             (BookingID, LastName, FirstName, MiddleName, Contact, Birthdate, Age)
                                             VALUES
                                             (@BookingID, @LastName, @FirstName, @MiddleName, @Contact, @Birthdate, @Age)";
                        using (var command = new SqlCommand(insertPax, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookingID",   newBookingID);
                            command.Parameters.AddWithValue("@LastName",    request.LastName);
                            command.Parameters.AddWithValue("@FirstName",   request.FirstName);
                            command.Parameters.AddWithValue("@MiddleName",  request.MiddleName);
                            command.Parameters.AddWithValue("@Contact",     request.Contact);
                            command.Parameters.AddWithValue("@Birthdate",   request.Birthdate);
                            command.Parameters.AddWithValue("@Age",         request.Age);
                            command.ExecuteNonQuery();
                        }

                        // 3. Increment CurrentPax
                        string updatePax = "UPDATE FD_Details SET CurrentPax = CurrentPax + 1 WHERE FlightID = @FlightID";
                        using (var command = new SqlCommand(updatePax, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FlightID", request.FlightID);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return newBookingID;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool UpdatePassenger(int bookingId, BookingPax pax)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE Booking_Pax
                                 SET LastName  = @LastName,  FirstName = @FirstName,
                                     Contact   = @Contact,   Birthdate = @Birthdate, Age = @Age
                                 WHERE BookingID = @BookingID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    command.Parameters.AddWithValue("@LastName",  pax.LastName);
                    command.Parameters.AddWithValue("@FirstName", pax.FirstName);
                    command.Parameters.AddWithValue("@Contact",   pax.Contact);
                    command.Parameters.AddWithValue("@Birthdate", pax.Birthdate);
                    command.Parameters.AddWithValue("@Age",       pax.Age);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool TransferFlight(int bookingId, int newFlightId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Get old FlightID
                        int oldFlightId;
                        string getOld = "SELECT FlightID FROM Bookings WHERE BookingID = @BookingID";
                        using (var command = new SqlCommand(getOld, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookingID", bookingId);
                            oldFlightId = Convert.ToInt32(command.ExecuteScalar());
                        }

                        // Update Booking FlightID
                        string updateBooking = "UPDATE Bookings SET FlightID = @NewFlightID WHERE BookingID = @BookingID";
                        using (var command = new SqlCommand(updateBooking, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@NewFlightID", newFlightId);
                            command.Parameters.AddWithValue("@BookingID",   bookingId);
                            command.ExecuteNonQuery();
                        }

                        // Decrement old flight
                        string decOld = "UPDATE FD_Details SET CurrentPax = CurrentPax - 1 WHERE FlightID = @FlightID";
                        using (var command = new SqlCommand(decOld, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FlightID", oldFlightId);
                            command.ExecuteNonQuery();
                        }

                        // Increment new flight
                        string incNew = "UPDATE FD_Details SET CurrentPax = CurrentPax + 1 WHERE FlightID = @FlightID";
                        using (var command = new SqlCommand(incNew, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FlightID", newFlightId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public bool CancelBooking(int bookingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Get FlightID
                        int flightId;
                        string getFlight = "SELECT FlightID FROM Bookings WHERE BookingID = @BookingID";
                        using (var command = new SqlCommand(getFlight, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookingID", bookingId);
                            var result = command.ExecuteScalar();
                            if (result == null) return false;
                            flightId = Convert.ToInt32(result);
                        }

                        // Soft delete Confirmation
                        string cancelConf = "UPDATE Confirmation SET Status = 'Cancelled' WHERE BookingID = @BookingID";
                        using (var command = new SqlCommand(cancelConf, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@BookingID", bookingId);
                            command.ExecuteNonQuery();
                        }

                        // Decrement CurrentPax
                        string decPax = "UPDATE FD_Details SET CurrentPax = CurrentPax - 1 WHERE FlightID = @FlightID";
                        using (var command = new SqlCommand(decPax, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@FlightID", flightId);
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
