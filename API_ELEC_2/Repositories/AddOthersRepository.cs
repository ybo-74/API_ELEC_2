using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class AddOthersRepository
    {
        private readonly string _connectionString;

        public AddOthersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

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

        private AddOthers MapAddOthers(SqlDataReader reader)
        {
            return new AddOthers
            {
                AddOthersID  = (int)reader["AddOthersID"],
                BookingID    = (int)reader["BookingID"],
                PaxID        = (int)reader["PaxID"],
                MealID       = reader["MealID"]      == DBNull.Value ? 0 : (int)reader["MealID"],
                SeatID       = reader["SeatID"]      == DBNull.Value ? 0 : (int)reader["SeatID"],
                BaggageID    = reader["BaggageID"]   == DBNull.Value ? 0 : (int)reader["BaggageID"],
                InsuranceID  = reader["InsuranceID"] == DBNull.Value ? 0 : (int)reader["InsuranceID"],
                IsMeal       = (bool)reader["IsMeal"],
                IsSeat       = (bool)reader["IsSeat"],
                IsBaggage    = (bool)reader["IsBaggage"],
                IsInsurance  = (bool)reader["IsInsurance"],
                IsActive     = (bool)reader["IsActive"]
            };
        }

        public AddOthers GetByID(int id)
        {
            AddOthers item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM AddOthers WHERE AddOthersID = @AddOthersID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AddOthersID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = MapAddOthers(reader);
                    }
                }
            }
            return item;
        }

        
        public IEnumerable<AddOthers> GetByBookingID(int bookingId)
        {
            var list = new List<AddOthers>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM AddOthers WHERE BookingID = @BookingID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID", bookingId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapAddOthers(reader));
                    }
                }
            }
            return list;
        }

        
        public IEnumerable<AddOthers> GetByPaxID(int paxId)
        {
            var list = new List<AddOthers>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM AddOthers WHERE PaxID = @PaxID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PaxID", paxId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            list.Add(MapAddOthers(reader));
                    }
                }
            }
            return list;
        }

        public bool AddAddOthers(AddOthers item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO AddOthers
                                 (BookingID, PaxID, MealID, SeatID, BaggageID, InsuranceID,
                                  IsMeal, IsSeat, IsBaggage, IsInsurance, IsActive)
                                 VALUES
                                 (@BookingID, @PaxID, @MealID, @SeatID, @BaggageID, @InsuranceID,
                                  @IsMeal, @IsSeat, @IsBaggage, @IsInsurance, 1)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BookingID",   item.BookingID);
                    command.Parameters.AddWithValue("@PaxID",       item.PaxID);
                    command.Parameters.AddWithValue("@MealID",      item.MealID == 0 ? (object)DBNull.Value : item.MealID);
                    command.Parameters.AddWithValue("@SeatID",      item.SeatID == 0 ? (object)DBNull.Value : item.SeatID);
                    command.Parameters.AddWithValue("@BaggageID",   item.BaggageID == 0 ? (object)DBNull.Value : item.BaggageID);
                    command.Parameters.AddWithValue("@InsuranceID", item.InsuranceID == 0 ? (object)DBNull.Value : item.InsuranceID);
                    command.Parameters.AddWithValue("@IsMeal",      item.IsMeal);
                    command.Parameters.AddWithValue("@IsSeat",      item.IsSeat);
                    command.Parameters.AddWithValue("@IsBaggage",   item.IsBaggage);
                    command.Parameters.AddWithValue("@IsInsurance", item.IsInsurance);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAddOthers(AddOthers item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE AddOthers
                                 SET MealID = @MealID, SeatID = @SeatID,
                                     BaggageID = @BaggageID, InsuranceID = @InsuranceID,
                                     IsMeal = @IsMeal, IsSeat = @IsSeat,
                                     IsBaggage = @IsBaggage, IsInsurance = @IsInsurance
                                 WHERE AddOthersID = @AddOthersID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AddOthersID", item.AddOthersID);
                    command.Parameters.AddWithValue("@MealID",      item.MealID == 0 ? (object)DBNull.Value : item.MealID);
                    command.Parameters.AddWithValue("@SeatID",      item.SeatID == 0 ? (object)DBNull.Value : item.SeatID);
                    command.Parameters.AddWithValue("@BaggageID",   item.BaggageID == 0 ? (object)DBNull.Value : item.BaggageID);
                    command.Parameters.AddWithValue("@InsuranceID", item.InsuranceID == 0 ? (object)DBNull.Value : item.InsuranceID);
                    command.Parameters.AddWithValue("@IsMeal",      item.IsMeal);
                    command.Parameters.AddWithValue("@IsSeat",      item.IsSeat);
                    command.Parameters.AddWithValue("@IsBaggage",   item.IsBaggage);
                    command.Parameters.AddWithValue("@IsInsurance", item.IsInsurance);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

     
        public bool SoftDelete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE AddOthers SET IsActive = 0 WHERE AddOthersID = @AddOthersID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AddOthersID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
