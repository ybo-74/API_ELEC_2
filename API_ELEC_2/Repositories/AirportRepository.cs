using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class AirportRepository
    {
        private readonly string _connectionString;

        public AirportRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        public IEnumerable<Airport> GetAllAirports()
        {
            var airports = new List<Airport>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT OtherDetailsID, Code, Description FROM FD_OtherDetails";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        airports.Add(new Airport
                        {
                            OtherDetailsID = (int)reader["OtherDetailsID"],
                            Code = reader["Code"].ToString() ?? string.Empty,
                            Description = reader["Description"].ToString() ?? string.Empty
                        });
                    }
                }
            }
            return airports;
        }

        public Airport GetAirportByID(int id)
        {
            Airport airport = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT OtherDetailsID, Code, Description FROM FD_OtherDetails WHERE OtherDetailsID = @OtherDetailsID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OtherDetailsID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            airport = new Airport
                            {
                                OtherDetailsID = (int)reader["OtherDetailsID"],
                                Code = reader["Code"].ToString() ?? string.Empty,
                                Description = reader["Description"].ToString() ?? string.Empty
                            };
                        }
                    }
                }
            }
            return airport;
        }

        public bool AddAirport(Airport airport)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "INSERT INTO FD_OtherDetails (Code, Description) VALUES (@Code, @Description)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Code", airport.Code);
                    command.Parameters.AddWithValue("@Description", airport.Description);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateAirport(Airport airport)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "UPDATE FD_OtherDetails SET Code = @Code, Description = @Description WHERE OtherDetailsID = @OtherDetailsID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OtherDetailsID", airport.OtherDetailsID);
                    command.Parameters.AddWithValue("@Code", airport.Code);
                    command.Parameters.AddWithValue("@Description", airport.Description);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteAirport(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM FD_OtherDetails WHERE OtherDetailsID = @OtherDetailsID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OtherDetailsID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
