using Microsoft.Data.SqlClient;
using API_ELEC_2.Models;

namespace API_ELEC_2.Repositories
{
    public class FlightRepository
    {
        private readonly string _connectionString;

        public FlightRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        private const string JoinQuery = @"
            SELECT f.FlightID, f.OriginID, f.DestinationID, f.AirlineID,
                   f.TravelDate, f.TravelTime, f.MaxPax, f.CurrentPax,
                   a.AirlineCode, a.Description AS AirlineName,
                   o.Code AS OriginCode, o.Description AS OriginName,
                   d.Code AS DestinationCode, d.Description AS DestinationName
            FROM FD_Details f
            JOIN FD_Airline a        ON f.AirlineID     = a.AirlineID
            JOIN FD_OtherDetails o   ON f.OriginID      = o.OtherDetailsID
            JOIN FD_OtherDetails d   ON f.DestinationID = d.OtherDetailsID";

        private Flight MapFlight(SqlDataReader reader)
        {
            return new Flight
            {
                FlightID        = (int)reader["FlightID"],
                OriginID        = (int)reader["OriginID"],
                DestinationID   = (int)reader["DestinationID"],
                AirlineID       = (int)reader["AirlineID"],
                TravelDate      = Convert.ToDateTime(reader["TravelDate"]),
                TravelTime      = reader["TravelTime"]?.ToString() ?? string.Empty,
                MaxPax          = (int)reader["MaxPax"],
                CurrentPax      = (int)reader["CurrentPax"],
                AirlineCode     = reader["AirlineCode"]?.ToString() ?? string.Empty,
                AirlineName     = reader["AirlineName"]?.ToString() ?? string.Empty,
                OriginCode      = reader["OriginCode"]?.ToString() ?? string.Empty,
                OriginName      = reader["OriginName"]?.ToString() ?? string.Empty,
                DestinationCode = reader["DestinationCode"]?.ToString() ?? string.Empty,
                DestinationName = reader["DestinationName"]?.ToString() ?? string.Empty
            };
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            var flights = new List<Flight>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(JoinQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        flights.Add(MapFlight(reader));
                }
            }
            return flights;
        }

        public IEnumerable<Flight> GetCompletedFlights()
        {
            var flights = new List<Flight>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = JoinQuery + " WHERE f.TravelDate < CAST(GETDATE() AS DATE)";
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                        flights.Add(MapFlight(reader));
                }
            }
            return flights;
        }

        public Flight GetFlightByID(int id)
        {
            Flight flight = null;
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = JoinQuery + " WHERE f.FlightID = @FlightID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                            flight = MapFlight(reader);
                    }
                }
            }
            return flight;
        }

        public bool AddFlight(Flight flight)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO FD_Details (OriginID, DestinationID, TravelTime, TravelDate, AirlineID, MaxPax, CurrentPax)
                                 VALUES (@OriginID, @DestinationID, @TravelTime, @TravelDate, @AirlineID, @MaxPax, 0)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OriginID",      flight.OriginID);
                    command.Parameters.AddWithValue("@DestinationID", flight.DestinationID);
                    command.Parameters.AddWithValue("@TravelTime",    flight.TravelTime);
                    command.Parameters.AddWithValue("@TravelDate",    flight.TravelDate);
                    command.Parameters.AddWithValue("@AirlineID",     flight.AirlineID);
                    command.Parameters.AddWithValue("@MaxPax",        flight.MaxPax);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool UpdateFlight(Flight flight)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE FD_Details
                                 SET OriginID = @OriginID, DestinationID = @DestinationID,
                                     TravelTime = @TravelTime, TravelDate = @TravelDate,
                                     AirlineID = @AirlineID, MaxPax = @MaxPax
                                 WHERE FlightID = @FlightID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID",      flight.FlightID);
                    command.Parameters.AddWithValue("@OriginID",      flight.OriginID);
                    command.Parameters.AddWithValue("@DestinationID", flight.DestinationID);
                    command.Parameters.AddWithValue("@TravelTime",    flight.TravelTime);
                    command.Parameters.AddWithValue("@TravelDate",    flight.TravelDate);
                    command.Parameters.AddWithValue("@AirlineID",     flight.AirlineID);
                    command.Parameters.AddWithValue("@MaxPax",        flight.MaxPax);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        public bool DeleteFlight(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM FD_Details WHERE FlightID = @FlightID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FlightID", id);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}
