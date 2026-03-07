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
    }
}
