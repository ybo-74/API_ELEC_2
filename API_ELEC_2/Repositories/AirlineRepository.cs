using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class AirlineRepository
    {
        private readonly string _connectionString;

        public AirlineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<Airline> GetAllAirlines()
        {
            var airlines = new List<Airline>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT AirlineID, AirlineCode, Description FROM FD_Airline";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        airlines.Add(new Airline
                        {
                            AirlineID = (int)reader["AirlineID"],
                            AirlineCode = reader["AirlineCode"].ToString() ?? string.Empty,
                            Description = reader["Description"].ToString() ?? string.Empty
                        });
                    }
                }
            }
            return airlines;
        }

        public Airline GetAirlineByID(int id)
        {
            Airline airline = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT AirlineID, AirlineCode, Description FROM FD_Airline WHERE AirlineID = @AirlineID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirlineID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            airline = new Airline
                            {
                                AirlineID = (int)reader["AirlineID"],
                                AirlineCode = reader["AirlineCode"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return airline;
        }

        public bool AddAirline(Airline airline)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO FD_Airline (AirlineCode, Description) VALUES (@AirlineCode, @Description)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirlineCode", airline.AirlineCode);
                    command.Parameters.AddWithValue("@Description", airline.Description);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAirline(Airline airline)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE FD_Airline SET AirlineCode = @AirlineCode, Description = @Description WHERE AirlineID = @AirlineID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirlineID", airline.AirlineID);
                    command.Parameters.AddWithValue("@AirlineCode", airline.AirlineCode);
                    command.Parameters.AddWithValue("@Description", airline.Description);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteAirline(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM FD_Airline WHERE AirlineID = @AirlineID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AirlineID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
