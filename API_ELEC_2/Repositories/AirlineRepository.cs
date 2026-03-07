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
    }
}
