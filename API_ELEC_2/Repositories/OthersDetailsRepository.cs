using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class OthersDetailsRepository
    {
        private readonly string _connectionString;

        public OthersDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        // Check if booking is cancelled before allowing POST/PUT
        public bool IsBookingCancelled(int bookingId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"SELECT COUNT(*) FROM Confirmation 
                                 WHERE BookingID = @BookingID AND Status = 'Cancelled'";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    return Convert.ToInt32(command.ExecuteScalar()) > 0;
                }
            }
        }

        private OthersDetails MapOthersDetails(SqlDataReader reader)
        {
            return new OthersDetails
            {
                OthersID    = (int)reader["OthersID"],
                BookingID   = (int)reader["BookingID"],
                MealID      = reader["MealID"]      == DBNull.Value ? 0 : (int)reader["MealID"],
                InsuranceID = reader["InsuranceID"] == DBNull.Value ? 0 : (int)reader["InsuranceID"],
                SeatID      = reader["SeatID"]      == DBNull.Value ? 0 : (int)reader["SeatID"],
                BaggageID   = reader["BaggageID"]   == DBNull.Value ? 0 : (int)reader["BaggageID"],
                PaxID       = reader["PaxID"]       == DBNull.Value ? 0 : (int)reader["PaxID"]
            };
        }

        public IEnumerable<OthersDetails> GetAll()
        {
            var list = new List<OthersDetails>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM OthersDetails", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(MapOthersDetails(reader));
                }
            }
            return list;
        }

        public OthersDetails GetByID(int id)
        {
            OthersDetails item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM OthersDetails WHERE OthersID = @OthersID", connection))
                {
                    command.Parameters.AddWithValue("@OthersID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = MapOthersDetails(reader);
                    }
                }
            }
            return item;
        }

        public IEnumerable<OthersDetails> GetByBookingID(int bookingId)
        {
            var list = new List<OthersDetails>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM OthersDetails WHERE BookingID = @BookingID", connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapOthersDetails(reader));
                    }
                }
            }
            return list;
        }

        public bool Add(OthersDetails item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO OthersDetails (BookingID, MealID, InsuranceID, SeatID, BaggageID, PaxID)
                                 VALUES (@BookingID, @MealID, @InsuranceID, @SeatID, @BaggageID, @PaxID)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID",   item.BookingID);
                    command.Parameters.AddWithValue("@MealID",      item.MealID      == 0 ? (object)DBNull.Value : item.MealID);
                    command.Parameters.AddWithValue("@InsuranceID", item.InsuranceID == 0 ? (object)DBNull.Value : item.InsuranceID);
                    command.Parameters.AddWithValue("@SeatID",      item.SeatID      == 0 ? (object)DBNull.Value : item.SeatID);
                    command.Parameters.AddWithValue("@BaggageID",   item.BaggageID   == 0 ? (object)DBNull.Value : item.BaggageID);
                    command.Parameters.AddWithValue("@PaxID",       item.PaxID       == 0 ? (object)DBNull.Value : item.PaxID);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(OthersDetails item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE OthersDetails
                                 SET MealID = @MealID, InsuranceID = @InsuranceID,
                                     SeatID = @SeatID, BaggageID = @BaggageID, PaxID = @PaxID
                                 WHERE OthersID = @OthersID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OthersID",    item.OthersID);
                    command.Parameters.AddWithValue("@MealID",      item.MealID      == 0 ? (object)DBNull.Value : item.MealID);
                    command.Parameters.AddWithValue("@InsuranceID", item.InsuranceID == 0 ? (object)DBNull.Value : item.InsuranceID);
                    command.Parameters.AddWithValue("@SeatID",      item.SeatID      == 0 ? (object)DBNull.Value : item.SeatID);
                    command.Parameters.AddWithValue("@BaggageID",   item.BaggageID   == 0 ? (object)DBNull.Value : item.BaggageID);
                    command.Parameters.AddWithValue("@PaxID",       item.PaxID       == 0 ? (object)DBNull.Value : item.PaxID);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM OthersDetails WHERE OthersID = @OthersID", connection))
                {
                    command.Parameters.AddWithValue("@OthersID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
