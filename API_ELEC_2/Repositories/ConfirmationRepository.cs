using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class ConfirmationRepository
    {
        private readonly string _connectionString;

        public ConfirmationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        private Confirmation MapConfirmation(SqlDataReader reader)
        {
            return new Confirmation
            {
                ConfirmationID   = (int)reader["ConfirmationID"],
                BookingID        = (int)reader["BookingID"],
                FlightID         = (int)reader["FlightID"],
                ConfirmationCode = reader["ConfirmationCode"].ToString() ?? string.Empty,
                Status           = reader["Status"].ToString()           ?? string.Empty,
                ConfirmationDate = Convert.ToDateTime(reader["ConfirmationDate"])
            };
        }

        public IEnumerable<Confirmation> GetAllConfirmations()
        {
            var list = new List<Confirmation>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ConfirmationID, BookingID, FlightID, ConfirmationCode, Status, ConfirmationDate FROM Confirmation";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(MapConfirmation(reader));
                }
            }
            return list;
        }

        public IEnumerable<Confirmation> GetByBookingID(int bookingId)
        {
            var list = new List<Confirmation>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ConfirmationID, BookingID, FlightID, ConfirmationCode, Status, ConfirmationDate FROM Confirmation WHERE BookingID = @BookingID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapConfirmation(reader));
                    }
                }
            }
            return list;
        }

        public Confirmation GetByID(int id)
        {
            Confirmation item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT ConfirmationID, BookingID, FlightID, ConfirmationCode, Status, ConfirmationDate FROM Confirmation WHERE ConfirmationID = @ConfirmationID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ConfirmationID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = MapConfirmation(reader);
                    }
                }
            }
            return item;
        }

        public bool Add(CreateConfirmationRequest request)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Confirmation (BookingID, FlightID, ConfirmationCode, Status, ConfirmationDate)
                                 VALUES (@BookingID, @FlightID, @ConfirmationCode, 'Confirmed', GETDATE())";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID",        request.BookingID);
                    command.Parameters.AddWithValue("@FlightID",         request.FlightID);
                    command.Parameters.AddWithValue("@ConfirmationCode", request.ConfirmationCode);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(Confirmation confirmation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE Confirmation
                                 SET BookingID = @BookingID, FlightID = @FlightID,
                                     ConfirmationCode = @ConfirmationCode, Status = @Status
                                 WHERE ConfirmationID = @ConfirmationID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ConfirmationID",   confirmation.ConfirmationID);
                    command.Parameters.AddWithValue("@BookingID",        confirmation.BookingID);
                    command.Parameters.AddWithValue("@FlightID",         confirmation.FlightID);
                    command.Parameters.AddWithValue("@ConfirmationCode", confirmation.ConfirmationCode);
                    command.Parameters.AddWithValue("@Status",           confirmation.Status);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // Soft delete
        public bool SoftDelete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE Confirmation SET Status = 'Cancelled' WHERE ConfirmationID = @ConfirmationID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ConfirmationID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
