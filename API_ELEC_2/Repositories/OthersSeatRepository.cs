using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class OthersSeatRepository
    {
        private readonly string _connectionString;

        public OthersSeatRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<OthersSeat> GetAllSeats()
        {
            var list = new List<OthersSeat>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Seat FROM OthersSeat", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        list.Add(new OthersSeat { ID = (int)reader["ID"], Seat = reader["Seat"].ToString() ?? string.Empty });
                }
            }
            return list;
        }

        public OthersSeat GetByID(int id)
        {
            OthersSeat item = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT ID, Seat FROM OthersSeat WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            item = new OthersSeat { ID = (int)reader["ID"], Seat = reader["Seat"].ToString() ?? string.Empty };
                    }
                }
            }
            return item;
        }

        public bool Add(OthersSeat seat)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO OthersSeat (Seat) VALUES (@Seat)", connection))
                {
                    command.Parameters.AddWithValue("@Seat", seat.Seat);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Update(OthersSeat seat)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE OthersSeat SET Seat = @Seat WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", seat.ID);
                    command.Parameters.AddWithValue("@Seat", seat.Seat);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM OthersSeat WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
